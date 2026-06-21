using CleanArchitecture.Application.Common.Trace;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Api.Framework.Extensions;

public static class TraceExtensions
{
    public static string? GetTraceToken(this HttpContext context)
    {
        return context.Items.TryGetValue(TraceConstants.TraceToken, out var token)
            ? token as string
            : null;
    }
}
