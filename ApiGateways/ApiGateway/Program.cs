using ApiGateway.Configuration;
using ApiGateway.Extensions;
using ApiGateway.Middleware;
using ApiGateway.Transforms;
using CorrelationId;
using CorrelationId.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
    });

// Logging con Serialog 
builder.Host.UseSerilog((ctx, config) =>
    config.ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console());

// YARP Reverse Proxy +
// DESPUÉS
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms<CustomHeaderTransform>()
    .ConfigureHttpClient((context, handler) =>
    {
        // Solo en desarrollo — acepta certificados self-signed de los microservicios
        handler.SslOptions.RemoteCertificateValidationCallback = 
            (sender, cert, chain, errors) => true;
    });

//UI Health Checks 
 builder.Services
     .AddHealthChecksUI()
     .AddInMemoryStorage();

builder.Services.AddHealthChecks();

// rate limiting 
builder.Services.AddGatewayRateLimiting();

// cors -- por el momento no seria la dirección de nuestro FrontEnd
//builder.Services.AddCors(options =>
//{

//});

// correlation id 
//builder.Services.AddDefaultCorrelationId(); 
builder.Services.AddDefaultCorrelationId();
builder.Services.Configure<CorrelationIdOptions>(options => {
    options.AddToLoggingScope = true;
    options.UpdateTraceIdentifier = true;
    options.RequestHeader = "X-Correlation-ID";
    options.ResponseHeader = "X-Correlation-ID";
    options.IncludeInResponse = true;
});

//Open Telemetry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
        resource.AddService(serviceName: "ApiGateway"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter()); 

var app = builder.Build();

app.UseCorrelationId();

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
app.UseMiddleware<GatewayJwtMiddleware>();

app.UseMiddleware<SwaggerProxyMiddleware>(); 

// validación jwt en api-gateway 

// reenviar jwt a los servicios para su autorización 
app.UseSerilogRequestLogging();
app.UseRateLimiter();
// app.UseAuthentication();
// app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(config =>
{
    config.UIPath = "/health-ui";
    config.ApiPath = "/health-ui-api";
});

app.MapReverseProxy(); // Enrutamiento tambien envia el token a los servicios para que tambien lo validen 

app.Run();
