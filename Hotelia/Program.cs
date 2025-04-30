using Hotelia.Shared;
using Hotelia.Shared.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));




builder.Services.AddServices(builder.Configuration);
var app = builder.Build();




app.UseMiddleware<RequestPerformanceMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("/", async (ILogger<Program> logger) =>
{
    return "Hello World!";
}).WithName("Reza")
.WithMetadata("Reza");


app.Run();