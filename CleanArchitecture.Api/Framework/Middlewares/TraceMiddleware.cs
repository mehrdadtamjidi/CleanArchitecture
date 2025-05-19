using CleanArchitecture.Application.Common.Trace;

public class TraceMiddleware
{
    private readonly RequestDelegate _next;

    public TraceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var traceToken = Guid.NewGuid().ToString("N");
        context.Items[TraceConstants.TraceToken] = traceToken;
        await _next(context);
    }
}

public static class TraceMiddlewareExtensions
{
    public static IApplicationBuilder UseTrace(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TraceMiddleware>();
    }
}
