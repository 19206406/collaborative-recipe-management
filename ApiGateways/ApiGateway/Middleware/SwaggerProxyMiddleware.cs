using System.Text;
using System.Text.Json;
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
        private readonly SwaggerAggregatorOptions _options = options.Value;
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

                var gatewayUrl = !string.IsNullOrWhiteSpace(_options.GatewayPublicUrl)
                    ? _options.GatewayPublicUrl.TrimEnd('/')
                    : $"{context.Request.Scheme}://{context.Request.Host}";

                var rewritten = RewriteServers(json, gatewayUrl);

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(rewritten);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error proxying Swagger JSON for service {ServiceName}", match.Name);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }

        private static string RewriteServers(string swaggerJson, string gatewayUrl)
        {
            using var doc = JsonDocument.Parse(swaggerJson);
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
            
            writer.WriteStartObject();

            foreach (var property in doc.RootElement.EnumerateObject())
            {
                if (property.Name == "servers")
                {
                    // Reemplazar completamente el array servers[]
                    writer.WritePropertyName("servers");
                    writer.WriteStartArray();
                    writer.WriteStartObject();
                    writer.WriteString("url", gatewayUrl);
                    writer.WriteString("description", "API Gateway");
                    writer.WriteEndObject();
                    writer.WriteEndArray();
                }
                else
                {
                    // Copiar el resto de propiedades sin modificar
                    property.WriteTo(writer);
                }
            }

            writer.WriteEndObject();
            writer.Flush();

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
