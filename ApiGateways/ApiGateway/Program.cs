using ApiGateway.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy")); 

var app = builder.Build();

// validación jwt en api-gateway 
app.UseMiddleware<GatewayJwtMiddleware>();

// reenviar jwt a los servicios para su autorización 
app.MapReverseProxy(); 

app.Run();
