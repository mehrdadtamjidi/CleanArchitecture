using Asp.Versioning;
using AutoMapper;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser;
using CleanArchitecture.Application.Features.V1.Users.Queries.LoginUser;
using CleanArchitecture.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class UserController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        public UserController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="request">User registration data</param>
        /// <returns>Registered user info</returns>
        [HttpPost("Register")]
        public async Task<ApiResult<CreateUserOutputDto>> Register(CreateUserInputDto request)
        {
            var command = mapper.Map<CreateUserCommand>(request);
            var response = await mediator.Send(command);
            return response; 
        }

        [HttpPost("Login")]
        public async Task<ApiResult<LoginUserOutputDto>> Login(LoginUserInputDto request)
        {
            var query = mapper.Map<LoginUserQuery>(request);
            var response = await mediator.Send(query);
            return response;
        }


        [HttpPost("GetUserById")]
        [Authorize]
        public async Task<ApiResult<CreateUserOutputDto>> GetUserById(CreateUserInputDto request)
        {
            var command = new CreateUserCommand { };
            var response = await mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("GetUsers")]
        [Authorize]
        public async Task<ApiResult<CreateUserOutputDto>> GetUsers(CreateUserInputDto request)
        {
            var command = new CreateUserCommand { };
            var response = await mediator.Send(command);
            return Ok(response);
        }

    }
}
