using BuildingBlocks.Behaviors;
using BuildingBlocks.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Notification.API;
using Notification.API.Common.Database;
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

// repositorios 
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationPreferenceRepository, NotificationPreferenceRepository>();

var app = builder.Build();

// migraciones en automatico 
await app.ApplyMigrationsAsync<NotificationDbContext>(); 

app.UseCors(); 

// validaciones 
app.UseExceptionHandler();
app.UseCustomExceptionHandler();

// Fastendpoints 
app.UseFastEndpoints();
app.UseSwaggerGen(); 

app.Run();
