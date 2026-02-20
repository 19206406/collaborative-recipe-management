using StackExchange.Redis;

namespace Recommendation.API.Common.Database
{
    public static class RedisConfiguration
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["Redis:ConnectionString"];

            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(connectionString));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = "recommendations:";
            });

            return services; 
        }
    }
}
