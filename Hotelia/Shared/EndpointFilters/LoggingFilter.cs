using Serilog;
using System.Diagnostics;
using System.Text;

namespace Hotelia.Shared.EndpointFilters;

public class LoggingFilter<T>(ILogger<T> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var result = await next(context);
        stopwatch.Stop();
        logger.LogInformation(LogBuilder(context, stopwatch));
        return result;
    }


    private string LogBuilder(EndpointFilterInvocationContext context, Stopwatch stopwatch)
    {
        var routName =context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<RouteNameMetadata>()?.RouteName;
        var message = new StringBuilder();
        message.Append(routName).Append("  took  ").Append(stopwatch.ElapsedMilliseconds).Append(" ms");
        return message.ToString();
    }
}