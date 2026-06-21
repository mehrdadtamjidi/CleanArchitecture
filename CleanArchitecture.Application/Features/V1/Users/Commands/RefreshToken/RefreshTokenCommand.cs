using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Responses;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<ApiResult<LoginUserOutputDto>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
