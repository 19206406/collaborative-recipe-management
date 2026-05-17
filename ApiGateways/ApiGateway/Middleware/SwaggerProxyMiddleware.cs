using ApiGateway.Configuration;
using Microsoft.Extensions.Options;

namespace ApiGateway.Middleware
{
    public class SwaggerProxyMiddleware(
        RequestDelegate next,
        IHttpClientFactory httpClientFactory,
        IOptions<SwaggerAggregatorOptions> options,
        ILogger<SwaggerProxyMiddleware> logger)
    {
        private readonly List<SwaggerServiceDefinition> _services = options.Value.Services;

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            var match = _services.FirstOrDefault(s =>
                path.Equals($"/swagger/{s.Name}/swagger.json", StringComparison.OrdinalIgnoreCase)); 
            
            if (match is null)
            {
                await next(context);
                return; 
            }

            try
            {
                var client = httpClientFactory.CreateClient("swagger-proxy");
                var response = await client.GetAsync(match.SwaggerUrl); 

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning(
                    "Failed to fetch Swagger JSON for {ServiceName}. Status: {StatusCode}",
                    match.Name, response.StatusCode);

                    context.Response.StatusCode = StatusCodes.Status502BadGateway;
                    await context.Response.WriteAsync($"Could not reach {match.DisplayName}");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error proxying Swagger JSON for service {ServiceName}", match.Name);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
