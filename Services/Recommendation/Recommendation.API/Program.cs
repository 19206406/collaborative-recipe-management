using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// configurar redis como caché distribuido
var redisConnectionString = builder.Configuration.GetConnectionString("Redis"); 
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<Program>>();

    try
    {
        var configuration = ConfigurationOptions.Parse(redisConnectionString);
        configuration.AbortOnConnectFail = false;
        configuration.ConnectTimeout = 5000;
        configuration.SyncTimeout = 5000;

        var connection = ConnectionMultiplexer.Connect(configuration);

        logger.LogInformation("Connected to Redis at {Endpoint}", redisConnectionString);

        return connection;
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to connect to Redis at {Endpoint}", redisConnectionString);
        throw;
    }
}); 

var app = builder.Build();


app.Run();
