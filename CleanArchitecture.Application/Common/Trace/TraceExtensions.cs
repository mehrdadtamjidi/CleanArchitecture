using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


namespace CleanArchitecture.Application.Common.Trace;

public static class TraceExtensions
{
    public static string? GetTraceToken(this HttpContext context)
    {
        if (context.Items.TryGetValue(TraceConstants.TraceToken, out var traceToken)) return (string?)traceToken;
        return null;
    }
}