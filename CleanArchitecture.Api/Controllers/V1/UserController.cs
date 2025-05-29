using Asp.Versioning;
using AutoMapper;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser;
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
        [Authorize]
        public async Task<ApiResult<CreateUserOutputDto>> Register(CreateUserInputDto request)
        {
            var command = mapper.Map<CreateUserCommand>(request);
            var response = await mediator.Send(command);
            return response; 
        }

        [HttpPost("Login")]
        public async Task<ApiResult<CreateUserOutputDto>> Login(CreateUserInputDto request)
        {
            var command = new CreateUserCommand { };
            var response = await mediator.Send(command);
            return Ok(response);
        }


        [HttpPost("GetUserById")]
        public async Task<ApiResult<CreateUserOutputDto>> GetUserById(CreateUserInputDto request)
        {
            var command = new CreateUserCommand { };
            var response = await mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("GetUsers")]
        public async Task<ApiResult<CreateUserOutputDto>> GetUsers(CreateUserInputDto request)
        {
            var command = new CreateUserCommand { };
            var response = await mediator.Send(command);
            return Ok(response);
        }

    }
}
