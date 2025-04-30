using Hotelia.Shared;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));




builder.Services.AddServices(builder.Configuration);
var app = builder.Build();
app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("First Log");
    return "Hello World!";
});


app.Run();