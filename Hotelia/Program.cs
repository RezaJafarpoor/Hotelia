using Hotelia;
using Hotelia.Shared.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(option =>
{
    option.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


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