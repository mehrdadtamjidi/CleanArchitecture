using AutoMapper;
using CleanArchitecture.Application.CustomMapping;
using CleanArchitecture.Application.Features.V1.Users.Queries.LoginUser;

namespace CleanArchitecture.Application.DTOs.V1.Users
{
    public class LoginUserInputDto : IHaveCustomMapping
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public void CreateMappings(Profile profile)
        {
            profile.CreateMap<LoginUserInputDto, LoginUserQuery>().ReverseMap();
        }
    }

    public class LoginUserOutputDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

    }
}
