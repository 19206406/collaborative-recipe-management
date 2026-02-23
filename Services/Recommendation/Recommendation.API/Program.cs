using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Recommendation.API;
using Recommendation.API.Common.Cache;
using Recommendation.API.Common.Database;
using Recommendation.API.Features.Clients.RatingClient;
using Recommendation.API.Features.Clients.RecipeClient;
using Recommendation.API.Features.Clients.UserClient;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Redis 
builder.Services.AddRedisCache(builder.Configuration);

// FastEndpoints 
builder.Services.AddFastEndpoints();

// Cache servicio propio 
builder.Services.AddSingleton<ICacheService, CacheService>();

// HTTP Clients 
var servicesUrls = builder.Configuration.GetSection("ServicesUrls");
var httpSettings = builder.Configuration.GetSection("HttpClientSettings");

// recipe client 
builder.Services.AddHttpClient<IRecipeServiceClient, RecipeServiceClient>(client =>
{
    var baseUrl = servicesUrls["RecipeServiceUrl"];
    client.BaseAddress = new Uri(baseUrl!);

    var timeoutSeconds = httpSettings.GetValue<int>("TimeoutSeconds", 30);
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

    client.DefaultRequestHeaders.Add("Accep", "application/json");
    client.DefaultRequestHeaders.Add("Recipe-Agent", "RecommendationService/1.0"); 
});

// user client 
builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(client =>
{
    var baseUrl = servicesUrls["UserServiceUrl"];
    client.BaseAddress = new Uri(baseUrl!);

    var timeoutSeconds = httpSettings.GetValue<int>("TimeoutSeconds", 30);
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

    client.DefaultRequestHeaders.Add("Accep", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "RecommendationService/1.0");
});

// rating client 
builder.Services.AddHttpClient<IRatingServiceClient, RatingServiceClient>(client =>
{
    var baseUrl = servicesUrls["RatingServiceUrl"];
    client.BaseAddress = new Uri(baseUrl!);

    var timeoutSeconds = httpSettings.GetValue<int>("TimeoutSeconds", 30);
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

    client.DefaultRequestHeaders.Add("Accep", "application/json");
    client.DefaultRequestHeaders.Add("Rating-Agent", "RecommendationService/1.0");
});

// validators 
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// swagger 
builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = s =>
    {
        s.Title = "recommendation-service-api";
        s.Version = "v1";
    };
    options.AutoTagPathSegmentIndex = 0;
});

builder.Services.AddProblemDetails(); 

var app = builder.Build();

// validationes 
app.UseExceptionHandler();
app.UseCustomExceptionHandler();

// FastEndpoints 
app.UseFastEndpoints();
app.UseSwaggerGen(); 

app.Run();
