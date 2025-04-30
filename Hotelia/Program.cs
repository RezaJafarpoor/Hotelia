using Hotelia;
using Hotelia.Shared.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServices(builder.Configuration);
WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestPerformanceMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.RegisterEndpoints();

app.Run();