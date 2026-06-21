using System.Net;

namespace CleanArchitecture.Application.Common.Exceptions
{
    public class LogicException : AppException
    {
        public LogicException(string? message = null)
            : base(message ?? string.Empty, HttpStatusCode.UnprocessableEntity) { }
    }
}
