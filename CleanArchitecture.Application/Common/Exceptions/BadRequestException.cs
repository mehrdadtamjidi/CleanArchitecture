using System.Net;

namespace CleanArchitecture.Application.Common.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string? message = null)
            : base(message ?? string.Empty, HttpStatusCode.BadRequest) { }
    }
}
