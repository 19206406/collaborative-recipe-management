using BuildingBlocks.Behaviors;
using BuildingBlocks.Jwt.Service;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Recipe.API;
using Recipe.API.Common.Database;
using Recipe.API.Repositories.IngredientRepository;
using Recipe.API.Repositories.RecipeRepository;
using Recipe.API.Repositories.StepRepository;
using Recipe.API.Repositories.TagRepository;
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

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); 

builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = s =>
    {
        s.Title = "recipe-api-services";
        s.Version = "v1";
    };
    options.AutoTagPathSegmentIndex = 0;
});

builder.Services.AddProblemDetails(); 

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IStepRepository, StepRepository>();
builder.Services.AddScoped<ITagRepository, TagRepositoy>(); 

builder.Services.AddJwtValidation(builder.Configuration); 

var app = builder.Build();

app.UseExceptionHandler();
app.UseCustomExceptionHandler();
//app.UseAuthorization(); // talvez hay que quitar

app.UseFastEndpoints();
app.UseSwaggerGen(); 

app.Run();
