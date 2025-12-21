using Microsoft.AspNetCore.Mvc.Filters;
using CleanArchitecture.Application.Common.Trace;

namespace CleanArchitecture.Api.Framework.Filters
{
    public class AddTraceHeaderResultFilter : IAsyncResultFilter
    {
        private const string HeaderName = "X-Trace-Token-Id";

        public async Task OnResultExecutionAsync(
            ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var traceToken = context.HttpContext.GetTraceToken();

            if (!string.IsNullOrWhiteSpace(traceToken))
            {
                context.HttpContext.Response.Headers[HeaderName] = traceToken;
            }

            // Continue MVC result pipeline (this is NOT middleware)
            await next();
        }
    }
}
