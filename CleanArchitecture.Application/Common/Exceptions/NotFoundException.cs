using System.Net;

namespace CleanArchitecture.Application.Common.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string? message = null)
            : base(message ?? string.Empty, HttpStatusCode.NotFound) { }
    }
}
