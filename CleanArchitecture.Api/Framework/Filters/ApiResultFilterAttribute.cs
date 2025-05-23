using CleanArchitecture.Application.Responses;
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
                System.Reflection.PropertyInfo pi = badRequestObjectResult.Value.GetType().GetProperty("Errors");
                Dictionary<string, string[]> errors = (Dictionary<string, string[]>)pi.GetValue(badRequestObjectResult.Value, null);

                var message = badRequestObjectResult.ToString();
                var errorMessage = errors.SelectMany(p => p.Value).Distinct();
                message = string.Join(" | ", errorMessage);

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
            else if (context.Result is ObjectResult objectResult && objectResult.StatusCode == null
                && !(objectResult.Value is ApiResult))
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult.Value);
                context.Result = new JsonResult(apiResult) { StatusCode = objectResult.StatusCode };
            }
            else if (context.Result is ObjectResult result)
            {
                if (result.Value is ApiResult apiResultValue)
                {
                    if (result.StatusCode == null)
                    {
                        if (apiResultValue.IsSuccess)
                        {
                            result.StatusCode = StatusCodes.Status200OK;
                        }
                        else
                        {
                            result.StatusCode = apiResultValue.StatusCode switch
                            {
                                ApiResultStatusCode.NotFound => StatusCodes.Status404NotFound,
                                ApiResultStatusCode.BadRequest => StatusCodes.Status400BadRequest,
                                ApiResultStatusCode.UnAuthorized => StatusCodes.Status401Unauthorized,
                                ApiResultStatusCode.ServerError => StatusCodes.Status500InternalServerError,
                                ApiResultStatusCode.LogicError => StatusCodes.Status500InternalServerError,
                                _ => StatusCodes.Status400BadRequest
                            };
                        }
                    }

                    context.Result = new JsonResult(apiResultValue)
                    {
                        StatusCode = result.StatusCode
                    };
                }
                else if (result.StatusCode == null)
                {
                    var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, result.Value);
                    context.Result = new JsonResult(apiResult)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
            }

            base.OnResultExecuting(context);
        }
    }
}
