using BuildingBlocks.Behaviors;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using User.API;
using User.API.Common.Database;
using User.API.repositories.UserPreferenceRepository;
using User.API.repositories.UserRespository;
using User.API.Services.Jwt;
using User.API.Services.PasswordHash;

var builder = WebApplication.CreateBuilder(args);

// FastEndpoints 
builder.Services.AddFastEndpoints(); 

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("RecipeUserDb")); 
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); 

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserPreferenceRespository, UserPreferenceRepository>();
builder.Services.AddScoped<IJwtService, JwtService>(); 
builder.Services.AddExceptionHandler<User.API.Exceptions.ValidationException>();
builder.Services.AddProblemDetails();

// password Hash 
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<IPasswordHasher<Object>, PasswordHasher<Object>>();


// swagger 
builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = s =>
    {
        s.Title = "user-service-api";
        s.Version = "v1";
    };
    options.AutoTagPathSegmentIndex = 0; 
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseCustomExceptionHandler(); 

// FastEndpoints 
app.UseFastEndpoints().UseSwaggerGen(); 

app.Run();
