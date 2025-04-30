using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;
using System.Text;

namespace Hotelia.Shared.Middlewares;

public class RequestPerformanceMiddleware(ILogger<RequestPerformanceMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        int upperLimit =2;
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        await next(context);
        stopWatch.Stop();
        
        if (stopWatch.Elapsed.Seconds < upperLimit)
        {
            logger.LogWarning(BuildLog(context, stopWatch.ElapsedMilliseconds, upperLimit) );
        }
            
        
    }


    private string BuildLog(HttpContext context, long requestTime, int limit)
    {
        var builder = new StringBuilder();
        var endpointName = context.GetEndpoint()?.Metadata.GetMetadata<RouteNameMetadata>()?.RouteName;
        builder
            .Append($"Request took More than {limit} seconds.  ")
            .Append("endpoint-name: ").Append(endpointName)
            .Append(" elapsed-milliseconds=").Append(requestTime.ToString()).Append(" ms");
        return builder.ToString();
    }
}