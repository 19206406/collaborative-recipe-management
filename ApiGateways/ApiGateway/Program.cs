using ApiGateway.Configuration;
using ApiGateway.Extensions;
using ApiGateway.Middleware;
using ApiGateway.Transforms;
using CorrelationId;
using CorrelationId.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration 
builder.Services.Configure<SwaggerAggregatorOptions>(
    builder.Configuration.GetSection(SwaggerAggregatorOptions.Section));

var swaggerOptions = builder.Configuration
    .GetSection(SwaggerAggregatorOptions.Section)
    .Get<SwaggerAggregatorOptions?>() ?? new();

// HttpClient para el proxy 
builder.Services.AddHttpClient("swagger-proxy")
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(10);
    });

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("gateway", new OpenApiInfo
//    {
//        Title = "API Gateway",
//        Version = "v1",
//        Description = "Gateway central de microservicios"
//    });
//});

// Logging con Serialog 
builder.Host.UseSerilog((ctx, config) =>
    config.ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console() // Consola obligatoria 
        .WriteTo.Seq("http://localhost:5341")); // UI 

// YARP Reverse Proxy +
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms<CustomHeaderTransform>(); 

// builder JWT 
builder.Services.AddGatewayAuthentication(builder.Configuration);

// Autorización  
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("authenticated", policy =>
        policy.RequireAuthenticatedUser()); 
});

// rate limiting 
builder.Services.AddGatewayRateLimiting();

// cors -- por el momento no seria la dirección de nuestro FrontEnd
//builder.Services.AddCors(options =>
//{

//});

// correlation id 
builder.Services.AddDefaultCorrelationId(); 
builder.Services.AddCorrelationId(options =>
{
    options.AddToLoggingScope = true;
    options.UpdateTraceIdentifier = true;
});

//// health checks 
//builder.Services.AddHealthChecks()
//    .AddUrlGroup(
//        new Uri("https://localhost:7296/health"), 
//        name: "UserService", 
//        tags: new[] { "service" })
//    .AddUrlGroup(
//        new Uri("https://localhost:7010/health"), 
//        name: "RecipeService", 
//        tags: new[] { "service" });

//Open Telemetry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
        resource.AddService(serviceName: "ApiGateway"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter()); 

var app = builder.Build();

// validación jwt en api-gateway 
app.UseMiddleware<GatewayJwtMiddleware>();

// reenviar jwt a los servicios para su autorización 
app.UseCorrelationId();
app.UseSerilogRequestLogging();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

//app.MapHealthChecks("/health", new HealthCheckOptions
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

app.UseSwaggerUI(c =>
{
    // Un endpoint por cada microservicio
    foreach (var service in swaggerOptions.Services)
    {
        c.SwaggerEndpoint(
            url: $"/swagger/{service.Name}/swagger.json",
            name: service.DisplayName
        );
    }

    c.RoutePrefix = "swagger"; // Accesible en /swagger
    c.DocumentTitle = "Microservicios - API Docs";
    c.DefaultModelsExpandDepth(-1); // Oculta los schemas por defecto (más limpio)
});

app.MapReverseProxy(); // Enrutamiento tambien envia el token a los servicios para que tambien lo validen 

app.Run();
