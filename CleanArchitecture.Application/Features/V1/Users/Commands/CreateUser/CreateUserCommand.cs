using AutoMapper;
using CleanArchitecture.Application.CustomMapping;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Responses;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<ApiResult<CreateUserOutputDto>>, IHaveCustomMapping
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public void CreateMappings(Profile profile)
        {
            profile.CreateMap<CreateUserCommand, User>();
        }
    }
}
