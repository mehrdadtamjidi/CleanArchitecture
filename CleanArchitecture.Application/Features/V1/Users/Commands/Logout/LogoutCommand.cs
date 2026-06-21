using CleanArchitecture.Application.Responses;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.Logout
{
    public class LogoutCommand : IRequest<ApiResult>
    {
        public int UserId { get; set; }
    }
}
