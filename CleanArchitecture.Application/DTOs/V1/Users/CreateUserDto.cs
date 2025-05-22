using AutoMapper;
using CleanArchitecture.Application.CustomMapping;
using CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs.V1.Users
{
    public class CreateUserInputDto : IHaveCustomMapping
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public void CreateMappings(Profile profile)
        {
            profile.CreateMap<CreateUserInputDto, CreateUserCommand>().ReverseMap();
        }
    }

    public class CreateUserOutputDto
    {
    }
}
