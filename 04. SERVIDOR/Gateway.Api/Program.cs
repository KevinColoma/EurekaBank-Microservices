var builder = WebApplication.CreateBuilder(args);

// API Gateway: enruta las peticiones /Eureka/* hacia los microservicios.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapGet("/", () => Results.Ok("EurekaBank API Gateway en ejecución."));

app.MapReverseProxy();

app.Run();
