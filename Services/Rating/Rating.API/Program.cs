using BuildingBlocks.Behaviors;
using BuildingBlocks.Extensions;
using BuildingBlocks.Jwt.Service;
using BuildingBlocks.Messaging.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rating.API;
using Rating.API.Common.Database;
using Rating.API.Features.Clients;
using Rating.API.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();

// RabbitMQ Messaging 
builder.Services.AddRabbitMQMessaging(builder.Configuration);

// login 
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// cors 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// http client 
var serviceUrls = builder.Configuration.GetSection("ServiceUrls");
var httpSettings = builder.Configuration.GetSection("HttpClientSettings");

builder.Services.AddHttpClient<IRecipesServiceClient, RecipesServiceClient>(client =>
{
    var baseUrl = serviceUrls["RecipesService"];
    client.BaseAddress = new Uri(baseUrl!);

    var timeoutSeconds = httpSettings.GetValue<int>("TimeoutSeconds", 30);
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "RatingsService/1.0");
});

// RabbitMQ --- publicador 
builder.Services.AddRabbitMQMessaging(builder.Configuration); 

// db context
builder.Services.AddDbContext<RatingDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("RatingDb"));
}); 

// mediatR 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// validators 
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// swagger 
builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = s =>
    {
        s.Title = "rating-service-api";
        s.Version = "v1";
    };
    options.AutoTagPathSegmentIndex = 0;
});

builder.Services.AddProblemDetails(); 

// repositorios 
builder.Services.AddScoped<IRatingRepository, RatingRepository>();

// jwt 
builder.Services.AddJwtValidation(builder.Configuration); 

var app = builder.Build();

// migración en automatico 
await app.ApplyMigrationsAsync<RatingDbContext>(); 

app.UseCors(); // cors 

// validaciones 
app.UseExceptionHandler();
app.UseCustomExceptionHandler(); 

// FastEndpoints 
app.UseFastEndpoints();
app.UseSwaggerGen(); 

app.Run();
