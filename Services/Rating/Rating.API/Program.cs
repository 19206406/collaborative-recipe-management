using BuildingBlocks.Behaviors;
using BuildingBlocks.Jwt.Service;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rating.API;
using Rating.API.Common.Database;
using Rating.API.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();

builder.Services.AddDbContext<RatingDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("RatingDb"));
}); 

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

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

builder.Services.AddScoped<IRatingRepository, RatingRepository>();

builder.Services.AddJwtValidation(builder.Configuration); 

var app = builder.Build();

app.UseExceptionHandler();
app.UseCustomExceptionHandler(); 

// FastEndpoints 
app.UseFastEndpoints();
app.UseSwaggerGen(); 

app.Run();
