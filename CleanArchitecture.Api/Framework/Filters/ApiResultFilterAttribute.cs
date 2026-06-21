using CleanArchitecture.Api.Framework.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanArchitecture.Api.Framework.Filters
{
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult)
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, okObjectResult.Value);
                context.Result = new JsonResult(apiResult) { StatusCode = okObjectResult.StatusCode };
            }
            else if (context.Result is OkResult okResult)
            {
                var apiResult = new ApiResult(true, ApiResultStatusCode.Success);
                context.Result = new JsonResult(apiResult) { StatusCode = okResult.StatusCode };
            }
            else if (context.Result is BadRequestResult badRequestResult)
            {
                var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest);
                context.Result = new JsonResult(apiResult) { StatusCode = badRequestResult.StatusCode };
            }
            else if (context.Result is BadRequestObjectResult badRequestObjectResult)
            {
                var pi = badRequestObjectResult.Value.GetType().GetProperty("Errors");
                var errors = (Dictionary<string, string[]>)pi.GetValue(badRequestObjectResult.Value, null);
                var message = string.Join(" | ", errors.SelectMany(p => p.Value).Distinct());

                var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest, message);
                context.Result = new JsonResult(apiResult) { StatusCode = StatusCodes.Status400BadRequest };
            }
            else if (context.Result is ContentResult contentResult)
            {
                var apiResult = new ApiResult(true, ApiResultStatusCode.Success, contentResult.Content);
                context.Result = new JsonResult(apiResult) { StatusCode = contentResult.StatusCode };
            }
            else if (context.Result is NotFoundResult notFoundResult)
            {
                var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound);
                context.Result = new JsonResult(apiResult) { StatusCode = notFoundResult.StatusCode };
            }
            else if (context.Result is NotFoundObjectResult notFoundObjectResult)
            {
                var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value);
                context.Result = new JsonResult(apiResult) { StatusCode = notFoundObjectResult.StatusCode };
            }
            else if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value is ApiResult existingApiResult)
                {
                    if (objectResult.StatusCode == null)
                    {
                        objectResult.StatusCode = existingApiResult.IsSuccess
                            ? StatusCodes.Status200OK
                            : existingApiResult.StatusCode switch
                            {
                                ApiResultStatusCode.NotFound => StatusCodes.Status404NotFound,
                                ApiResultStatusCode.BadRequest => StatusCodes.Status400BadRequest,
                                ApiResultStatusCode.UnAuthorized => StatusCodes.Status401Unauthorized,
                                ApiResultStatusCode.LogicError => StatusCodes.Status422UnprocessableEntity,
                                _ => StatusCodes.Status500InternalServerError
                            };
                    }
                    context.Result = new JsonResult(existingApiResult) { StatusCode = objectResult.StatusCode };
                }
                else
                {
                    var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult.Value);
                    context.Result = new JsonResult(apiResult) { StatusCode = StatusCodes.Status200OK };
                }
            }

            base.OnResultExecuting(context);
        }
    }
}
