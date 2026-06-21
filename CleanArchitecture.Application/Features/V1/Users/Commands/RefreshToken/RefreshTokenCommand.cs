using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<LoginUserResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
