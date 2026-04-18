using BuildingBlocks.Behaviors;
using BuildingBlocks.Extensions;
using BuildingBlocks.Jwt.Service;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Notification.API;
using Notification.API.Common.Database;
using Notification.API.Features.Clients.RecipeClient;
using Notification.API.Features.Clients.UserClient;
using Notification.API.Repositories.NotificationPreferenceRepository;
using Notification.API.Repositories.NotificationRepository;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();

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

// db context 
builder.Services.AddDbContext<NotificationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NotificationDb"));
});

// mediatR 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// HTTP Clients 
var servicesUrls = builder.Configuration.GetSection("ServicesUrls");
var httpSettings = builder.Configuration.GetSection("HttpClientSettings");

// user client 
builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(client =>
{
    var baseUrl = servicesUrls["UserServiceUrl"];
    client.BaseAddress = new Uri(baseUrl!);

    var timeoutSeconds = httpSettings.GetValue<int>("TimeoutSeconds", 30);
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

    client.DefaultRequestHeaders.Add("Accep", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "NotificationService/1.0");
});

// recipe cliente 
builder.Services.AddHttpClient<IRecipeServiceClient, RecipeServiceClient>(client =>
{
    var baseUrl = servicesUrls["RecipeServiceUrl"];
    client.BaseAddress = new Uri(baseUrl!);

    var timeoutSeconds = httpSettings.GetValue<int>("TimeoutSeconds", 30);
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

    client.DefaultRequestHeaders.Add("Accep", "application/json");
    client.DefaultRequestHeaders.Add("Recipe-Agent", "NotificationService/1.0");
}); 

// validators 
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// swagger 
builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = s =>
    {
        s.Title = "notification-service-api";
        s.Version = "v1";
    };
    options.AutoTagPathSegmentIndex = 0;
});

builder.Services.AddProblemDetails();

builder.Services.AddJwtValidation(builder.Configuration); 

// repositorios 
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationPreferenceRepository, NotificationPreferenceRepository>();

var app = builder.Build();

// migraciones en automatico 
//await app.ApplyMigrationsAsync<NotificationDbContext>(); 

app.UseCors();

// jwt autenticación 
app.UseAuthentication();
app.UseAuthorization();

// validaciones 
app.UseExceptionHandler();
app.UseCustomExceptionHandler();

// Fastendpoints 
app.UseFastEndpoints();
app.UseSwaggerGen(); 

app.Run();
