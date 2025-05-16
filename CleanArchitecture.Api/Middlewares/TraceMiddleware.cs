using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using CleanArchitecture.Application.Common.Trace;

namespace CleanArchitecture.Api.Middlewares
{
    public static class TraceMiddlewareExtensions
    {
        public static IApplicationBuilder UseTrace(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TraceMiddleware>();
        }
    }
    public class TraceMiddleware
    {
        private readonly RequestDelegate _next;
        public TraceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITraceTokenGenerator? generator)
        {
            var endpoint = context.GetEndpoint();
            var attribute = endpoint?.Metadata.GetMetadata<CustomTraceTokenAttribute>();
            if (attribute != null)
            {
                generator = attribute.TraceTokenGenerator;
            }
            context.Items[TraceConstants.TraceToken] = generator?.GenerateToken();
            await _next.Invoke(context);
        }
    }
}
