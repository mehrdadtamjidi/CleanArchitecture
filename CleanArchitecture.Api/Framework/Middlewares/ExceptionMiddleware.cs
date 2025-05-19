using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json.Serialization;
using CleanArchitecture.Application.Common.Exceptions;


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

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string controllerName = context.Request.RouteValues["controller"]?.ToString();
            string actionName = context.Request.RouteValues["action"]?.ToString();

            context.Response.ContentType = "application/json";

            int statusCode;
            int apiStatusCode = 0;

            if (exception is AppException appException)
            {
                statusCode = (int)appException.HttpStatusCode;
                apiStatusCode = (int)appException.ApiStatusCode;
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                apiStatusCode = 0;
            }

            context.Response.StatusCode = statusCode;

            var errorModel = new ErrorResponseModel
            {
                IsSuccess = false,
                StatusCode = apiStatusCode,
                Message = exception.Message + exception.StackTrace,
                DateTime = DateTime.Now
            };

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(errorModel, serializerSettings);
            await context.Response.WriteAsync(json);
        }

    }
    public class ErrorResponseModel
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
    }
}
