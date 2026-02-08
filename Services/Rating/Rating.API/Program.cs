using BuildingBlocks.Behaviors;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
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

builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = s =>
    {
        s.Title = "rating-service-api";
        s.Version = "v1";
    };
    options.AutoTagPathSegmentIndex = 0;
});

builder.Services.AddScoped<IRatingRepository, RatingRepository>(); 

var app = builder.Build();

// FastEndpoints 
app.UseFastEndpoints().UseSwaggerGen(); 

app.Run();
