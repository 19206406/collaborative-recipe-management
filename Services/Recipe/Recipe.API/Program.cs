using BuildingBlocks.Behaviors;
using BuildingBlocks.Extensions;
using BuildingBlocks.Jwt.Service;
using BuildingBlocks.Messaging.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Recipe.API;
using Recipe.API.Common.Database;
using Recipe.API.Consumers;
using Recipe.API.Repositories.IngredientRepository;
using Recipe.API.Repositories.RecipeRepository;
using Recipe.API.Repositories.StepRepository;
using Recipe.API.Repositories.TagRepository;
using Recipe.API.Repositories.UnitOfWork;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();

// rabbitmq 
//builder.Services.AddRabbitMQMessaging(builder.Configuration);
//builder.Services.AddRabbitMQConsumer<RatingCreateAndUpdateConsumer>(); 
//builder.Services.AddRabbitMQConsumer<RatingDeleteConsumer>(); 

// dbContest 
builder.Services.AddDbContext<RecipeDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("RecipeDb")); 
});

// mediatr 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// validaciones 
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); 

// swagger 
builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = s =>
    {
        s.Title = "recipe-api-services";
        s.Version = "v1";
    };
    options.AutoTagPathSegmentIndex = 0;
});

// validaciones 
builder.Services.AddProblemDetails(); 

// repositorios FRom
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IStepRepository, StepRepository>();
builder.Services.AddScoped<ITagRepository, TagRepositoy>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 

// configuración de JWT para validación
builder.Services.AddJwtValidation(builder.Configuration); 

var app = builder.Build();

// migración en automatico 
//await app.ApplyMigrationsAsync<RecipeDbContext>(); 

// jwt autenticación 
app.UseAuthentication();
app.UseAuthorization(); 

// validaciones middleware
app.UseExceptionHandler();
app.UseCustomExceptionHandler();

// middleware de fastendpoints
app.UseFastEndpoints();
app.UseSwaggerGen(); 

app.Run();
