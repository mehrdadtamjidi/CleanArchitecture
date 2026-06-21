using CleanArchitecture.Api.Framework.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

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

            await next();
        }
    }
}
