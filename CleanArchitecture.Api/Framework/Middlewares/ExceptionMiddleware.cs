using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CleanArchitecture.Api.Framework.Middlewares
{
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }

    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            ApiResultStatusCode apiStatusCode;
            string message;

            if (exception is ValidationException validationException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                apiStatusCode = ApiResultStatusCode.BadRequest;

                var allErrors = validationException.Errors
                    .SelectMany(e => e.Value)
                    .Distinct()
                    .ToList();

                message = string.Join(" | ", allErrors);
            }
            else if (exception is AppException appException)
            {
                statusCode = (int)appException.HttpStatusCode;
                apiStatusCode = appException.ApiStatusCode;
                message = appException.Message;
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                apiStatusCode = ApiResultStatusCode.ServerError;

                message = _env.IsDevelopment()
                    ? $"{exception.Message} | {exception.StackTrace}"
                    : "An unexpected error occurred.";
            }

            context.Response.StatusCode = statusCode;

            var result = new ApiResult(false, apiStatusCode, message);
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(result, settings);
            await context.Response.WriteAsync(json);
        }
    }
}
