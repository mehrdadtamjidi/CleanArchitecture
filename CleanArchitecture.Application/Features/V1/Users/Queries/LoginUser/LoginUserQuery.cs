using AutoMapper;
using CleanArchitecture.Application.CustomMapping;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<LoginUserOutputDto>, IHaveCustomMapping
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public void CreateMappings(Profile profile)
        {
            profile.CreateMap<LoginUserQuery, User>();
        }
    }
}
