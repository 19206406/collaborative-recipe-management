using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace ApiGateway.Transforms
{
    public class CustomHeaderTransform : ITransformProvider
    {
        public void Apply(TransformBuilderContext context)
        {
            context.AddRequestTransform(async transformContext =>
            {
                var user = transformContext.HttpContext.User; 

                if (user.Identity?.IsAuthenticated == true)
                {
                    var userId = user.FindFirst("userId")?.Value;
                    var name = user.FindFirst("name")?.Value;

                    transformContext.ProxyRequest.Headers.TryAddWithoutValidation(
                        "X-User-Id", userId ?? string.Empty);

                    transformContext.ProxyRequest.Headers.TryAddWithoutValidation(
                        "X-User-Name", name ?? string.Empty);
                }

                if (transformContext.HttpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
                {
                    transformContext.ProxyRequest.Headers.TryAddWithoutValidation(
                        "X-Correlation-ID", correlationId.ToString()); 
                }
            });

            context.AddResponseTransform(transformContext =>
            {
                transformContext.ProxyResponse?.Headers.Remove("X-Powered-By");
                transformContext.ProxyResponse?.Headers.Remove("Server");
                return ValueTask.CompletedTask;
            }); 
        }

        public void ValidateCluster(TransformClusterValidationContext context)
        {
        }

        public void ValidateRoute(TransformRouteValidationContext context)
        {
        }
    }
}
