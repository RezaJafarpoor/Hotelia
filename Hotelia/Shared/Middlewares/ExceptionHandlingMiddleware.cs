using System.Globalization;
using System.Text;

namespace Hotelia.Shared.Middlewares;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            var builder = new StringBuilder();
            builder.Append(e.Message)
                .Append("  ")
                .Append(e.Message)
                .Append("  ")
                .Append(e.Message)
                .Append(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            logger.LogError(builder.ToString());
            var response = new
            {
                message = "Something went wrong",
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}