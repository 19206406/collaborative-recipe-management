using BuildingBlocks.Behaviors;
using BuildingBlocks.Jwt.Service;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Recipe.API.Common.Database;
using Recipe.API.Repositories;
using Recipe.API.Repositories.RepositoryInterfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();

builder.Services.AddDbContext<RecipeDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("RecipeDb")); 
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
        s.Title = "recipe-api-services";
        s.Version = "v1";
    };
    options.AutoTagPathSegmentIndex = 0;
});

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IStepRepository, StepRepository>(); 

builder.Services.AddJwtValidation(builder.Configuration); 

var app = builder.Build();

app.UseFastEndpoints();
app.UseSwaggerGen(); 

app.Run();
