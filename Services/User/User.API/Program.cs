using BuildingBlocks.Behaviors;
using BuildingBlocks.Extensions;
using BuildingBlocks.Jwt.Models;
using BuildingBlocks.Jwt.Service;
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

// db Context 
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("RecipeUserDb"));
});

// MediatR 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// validators 
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// repositories 
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserPreferenceRespository, UserPreferenceRepository>();

// servicio de jwt 
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddJwtValidation(builder.Configuration); // ← Esto agrega Authentication

// ← AGREGAR ESTO: Authorization se necesita cuando usas UseAuthorization() antes de FastEndpoints
builder.Services.AddAuthorization();

builder.Services.AddScoped<IJwtService, JwtService>();

// password Hash 
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<IPasswordHasher<Object>, PasswordHasher<Object>>();

builder.Services.AddProblemDetails();

// FastEndpoints
builder.Services.AddFastEndpoints();

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

// migracion de la base de datos en automatico 
//await app.ApplyMigrationsAsync<UserDbContext>();

// middlewares de excepciones 
app.UseExceptionHandler();
app.UseCustomExceptionHandler();

// ORDEN CORRECTO: Authentication → Authorization → FastEndpoints
app.UseAuthentication();
app.UseAuthorization();

// FastEndpoints
app.UseFastEndpoints()
   .UseSwaggerGen();

app.Run();