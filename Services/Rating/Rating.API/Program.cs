using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();

var app = builder.Build();

app.UseAuthorization();

app.Run();
