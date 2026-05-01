using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace BuildingBlocks.Extensions
{
    public static class MigrationExtensions
    {
        public static async Task ApplyMigrationsAsync<TContext>(this IApplicationBuilder app)
            where TContext : DbContext
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (exception, TimeSpan, attempt, _) =>
                    {
                        logger.LogWarning(
                            "[Migrations] Intento {Attempt}/5 fallido para {Context}. " +
                            "Reintentando en {Delay}s. Error: {Error}",
                            attempt, typeof(TContext).Name,
                            TimeSpan.TotalSeconds, exception.Message);
                    }
                );

            await retryPolicy.ExecuteAsync(async () =>
            {
                logger.LogInformation("[Migrations] Aplicando migracones para {Context}...",
                    typeof(TContext).Name);

                // verificar si la DB puede conectarse antes de migrar 
                var canConnect = await context.Database.CanConnectAsync();
                if (!canConnect)
                    throw new Exception($"No se puede conectar a la base de datos para {typeof(TContext).Name}");

                await context.Database.MigrateAsync();

                logger.LogInformation("[Migrations] Migraciones aplicadas correctamente para {Context}.", typeof(TContext).Name); 
            }); 
        }
    }
}
