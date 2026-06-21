using CleanArchitecture.Api.Framework.Responses;
using CleanArchitecture.Application.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CleanArchitecture.Api.Framework.Middlewares
{
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<GlobalExceptionMiddleware>();
    }

    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

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
                message = string.Join(" | ", validationException.Errors.SelectMany(e => e.Value).Distinct());
            }
            else if (exception is AppException appException)
            {
                statusCode = (int)appException.HttpStatusCode;
                apiStatusCode = MapToApiStatusCode(appException.HttpStatusCode);
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
            await context.Response.WriteAsync(JsonSerializer.Serialize(result, _jsonOptions));
        }

        private static ApiResultStatusCode MapToApiStatusCode(HttpStatusCode httpStatusCode) => httpStatusCode switch
        {
            HttpStatusCode.BadRequest => ApiResultStatusCode.BadRequest,
            HttpStatusCode.Unauthorized => ApiResultStatusCode.UnAuthorized,
            HttpStatusCode.Forbidden => ApiResultStatusCode.UnAuthorized,
            HttpStatusCode.NotFound => ApiResultStatusCode.NotFound,
            HttpStatusCode.UnprocessableEntity => ApiResultStatusCode.LogicError,
            _ => ApiResultStatusCode.ServerError
        };
    }
}
