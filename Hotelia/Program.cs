using Hotelia.Shared.Domain.Entities;
using Hotelia.Shared.Domain.Enums;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.Run();