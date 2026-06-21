using System.Net;

namespace CleanArchitecture.Application.Common.Exceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }

        public AppException(HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
            : base()
        {
            HttpStatusCode = httpStatusCode;
        }

        public AppException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public AppException(string message, Exception innerException, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
            : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
