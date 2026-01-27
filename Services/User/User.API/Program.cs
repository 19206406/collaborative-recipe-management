using BuildingBlocks.Behaviors;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using User.API.Common.Database;
using User.API.Exceptions;
using User.API.PasswordHash;
using User.API.repositories.UserPreferenceRepository;
using User.API.repositories.UserRespository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
//Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

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

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserPreferenceRespository, UserPreferenceRepository>(); 
builder.Services.AddExceptionHandler<ValidationException>();
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

// FastEndpoints 
app.UseFastEndpoints().UseSwaggerGen(); 

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
