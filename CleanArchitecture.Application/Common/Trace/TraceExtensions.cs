using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


namespace CleanArchitecture.Application.Common.Trace;

public static class TraceExtensions
{
    public static string? GetTraceToken(this HttpContext context)
    {
        return context.Items.TryGetValue(TraceConstants.TraceToken, out var token)
            ? token as string
            : null;
    }
}