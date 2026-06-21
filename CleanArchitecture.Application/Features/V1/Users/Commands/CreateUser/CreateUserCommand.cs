using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<CreateUserOutputDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }

        public static CreateUserCommand FromDto(CreateUserInputDto dto) => new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.UserName,
            Password = dto.Password,
            Gender = dto.Gender
        };
    }
}
