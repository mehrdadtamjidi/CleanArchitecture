using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<LoginUserOutputDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public static LoginUserQuery FromDto(LoginUserInputDto dto) => new()
        {
            UserName = dto.UserName,
            Password = dto.Password
        };
    }
}
