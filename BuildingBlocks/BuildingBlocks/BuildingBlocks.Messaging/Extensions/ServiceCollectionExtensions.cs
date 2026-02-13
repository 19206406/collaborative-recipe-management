using BuildingBlocks.Messaging.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static class IServiceCollectionExtensions
        {
            public static IServiceCollection AddRabbitMQMessaging(
                this IServiceCollection services, 
                IConfiguration configuration)
            {
                // configurar settings 
                services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

                // registrar publisher como singleton 
                services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

                return services; 
            }
        }

        public static IServiceCollection AddRabbitMQConsumer<TConsumer>(
            this IServiceCollection services) 
            where TConsumer : class, IHostedService
        {
            services.AddHostedService<TConsumer>();
            return services; 
        }
    }
}
