using Asp.Versioning;
using AutoMapper;
using CleanArchitecture.Application.DTOs.SharedModels;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser;
using CleanArchitecture.Application.Features.V1.Users.Queries.GetUser;
using CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers;
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
        [AllowAnonymous]
        public async Task<ApiResult<CreateUserOutputDto>> Register(CreateUserInputDto request)
        {
            var command = mapper.Map<CreateUserCommand>(request);
            var response = await mediator.Send(command);
            return response; 
        }

        /// <summary>
        /// Authenticates a user and returns a token if successful.
        /// </summary>
        /// <param name="request">User login credentials</param>
        /// <returns>Login result including JWT token</returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ApiResult<LoginUserOutputDto>> Login(LoginUserInputDto request)
        {
            var query = mapper.Map<LoginUserQuery>(request);
            var response = await mediator.Send(query);
            return response;
        }

        /// <summary>
        /// Retrieves a user by their unique ID.
        /// </summary>
        /// <param name="request">Object containing the user ID</param>
        /// <returns>User details</returns>
        [HttpPost("GetUserById")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<GetUserByIdOutputDto>> GetUserById(GetUserByIdDto request)
        {
            var query = new GetUserByIdQuery { Id = request.Id };
            var response = await mediator.Send(query);
            return response;  
        }

        /// <summary>
        /// Retrieves a paginated list of users.
        /// </summary>
        /// <param name="request">Pagination parameters (Page, PerPage)</param>
        /// <returns>Paginated list of user records</returns>
        [HttpPost("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PaginationResult<GetUsersOutputDto>>> GetUsers(GetUsersQuery request)
        {
            var query = new GetUsersQuery { Page = request.Page , PerPage = request.PerPage };
            var response = await mediator.Send(request);
            return response;
        }

    }
}
