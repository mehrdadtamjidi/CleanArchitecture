using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.Logout
{
    public class LogoutCommand : IRequest<Unit>
    {
        public int UserId { get; set; }
    }
}
