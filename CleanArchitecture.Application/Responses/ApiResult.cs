using CleanArchitecture.Application.Common;
using System.Text.Json.Serialization;

namespace CleanArchitecture.Application.Responses
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public ApiResultStatusCode StatusCode { get; set; }
        public DateTime DateTime { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string message = null)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message ?? statusCode.ToDisplay();
            DateTime = DateTime.UtcNow;
        }
    }

    public class ApiResult<TData> : ApiResult
        where TData : class
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TData Data { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData data, string message = null)
            : base(isSuccess, statusCode, message)
        {
            Data = data;
        }

        public static implicit operator ApiResult<TData>(TData data)
            => new(true, ApiResultStatusCode.Success, data);
    }
}
