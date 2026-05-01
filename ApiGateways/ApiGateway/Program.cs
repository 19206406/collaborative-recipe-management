using ApiGateway.Extensions;
using ApiGateway.Middleware;
using ApiGateway.Transforms;
using CorrelationId;
using CorrelationId.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging con Serialog 
builder.Host.UseSerilog((ctx, config) =>
    config.ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()); 

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

// health checks 

// correlation id 
builder.Services.AddCorrelationId(options =>
{
    options.AddToLoggingScope = true;
    options.UpdateTraceIdentifier = true;
}); 

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

app.MapReverseProxy(); 

app.Run();
