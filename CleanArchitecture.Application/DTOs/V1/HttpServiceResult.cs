using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs.V1
{
    public class HttpServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string? ReasonPhrase { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public string? RawResponse { get; set; }

        public static HttpServiceResult<T> Fail(string message, int statusCode = 0)
        {
            return new HttpServiceResult<T>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                ErrorMessage = message
            };
        }
    }
}
