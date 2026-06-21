using System.Net;

namespace CleanArchitecture.Application.Common.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string? message = null)
            : base(message ?? string.Empty, HttpStatusCode.Unauthorized) { }
    }
}
