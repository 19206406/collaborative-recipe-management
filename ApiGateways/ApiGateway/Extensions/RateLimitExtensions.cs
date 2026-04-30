using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace ApiGateway.Extensions
{
    public static class RateLimitExtensions
    {
        public static IServiceCollection AddGatewayRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 5
                    }));

                options.AddFixedWindowLimiter("auth-policy", opt =>
                {
                    opt.PermitLimit = 10;
                    opt.Window = TimeSpan.FromMinutes(1);
                });

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (ctx, token) =>
                {
                    ctx.HttpContext.Response.StatusCode = 429;
                    await ctx.HttpContext.Response.WriteAsync(
                        "Demasiadas solicitudes. Intenta más tarde.", token);
                }; 
            });

            return services; 
        }
    }
}
