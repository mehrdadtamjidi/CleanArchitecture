using AutoMapper;
using CleanArchitecture.Application.CustomMapping;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser;
using CleanArchitecture.Application.Responses;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<ApiResult<LoginUserOutputDto>>, IHaveCustomMapping
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public void CreateMappings(Profile profile)
        {
            profile.CreateMap<LoginUserQuery, User>();
        }
    }
}
