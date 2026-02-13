using BuildingBlocks.Messaging.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Messaging.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // servicios de RabbitMQ

        public static IServiceCollection AddRabbitMQMessaging(
            this IServiceCollection services, 
            IConfiguration configuration, 
            string sectionName = RabbitMQSettings.SectionName)
        {
            // registro de RabbitMQSettings 
            services.Configure<RabbitMQSettings>(
                configuration.GetSection(sectionName));

            // Registrar el publicador singleton 
            services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

            return services; 
        }

        // registrar consumidor de RabbitMQ como servicio de fondo
        public static IServiceCollection AddRabbitMQConsumer<TConsumer>(this IServiceCollection services)
            where TConsumer : class, IHostedService
        {
            services.AddHostedService<TConsumer>();
            return services; 
        }

        // registrar multiples consumidores de RabbitMQ como servicios de fondo 

        public static IServiceCollection AddRabbitMQConsumers(
            this IServiceCollection services,
            params Type[] consumerTypes)
        {
            foreach (var consumerType in consumerTypes)
            {
                if (!typeof(IHostedService).IsAssignableFrom(consumerType))
                    throw new ArgumentException($"El tipo {consumerType.Name} debe implementar IHostedService", nameof(consumerTypes));

                services.AddHostedService(
                    ServiceProvider => (IHostedService)ActivatorUtilities.CreateInstance(ServiceProvider, consumerType)); 
            }

            return services; 
        }
    }
}
