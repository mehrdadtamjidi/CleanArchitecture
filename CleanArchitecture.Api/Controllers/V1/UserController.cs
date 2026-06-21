using Asp.Versioning;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.DTOs.SharedModels;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser;
using CleanArchitecture.Application.Features.V1.Users.Commands.Logout;
using CleanArchitecture.Application.Features.V1.Users.Commands.RefreshToken;
using CleanArchitecture.Application.Features.V1.Users.Queries.GetUserById;
using CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers;
using CleanArchitecture.Application.Features.V1.Users.Commands.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Registers a new user in the system.</summary>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<CreateUserResponse>> Register(CreateUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        /// <summary>Authenticates a user and returns a token if successful.</summary>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginUserResponse>> Login(LoginUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        /// <summary>Issues a new access token using a valid refresh token.</summary>
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginUserResponse>> RefreshToken(RefreshTokenCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        /// <summary>Revokes all refresh tokens and invalidates the current session.</summary>
        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.Identity.GetUserId<int>();
            await _mediator.Send(new LogoutCommand { UserId = userId });
            return Ok();
        }

        /// <summary>Retrieves a user by their unique ID.</summary>
        [HttpPost("GetUserById")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetUserByIdResponse>> GetUserById(GetUserByIdQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        /// <summary>Retrieves a paginated list of users.</summary>
        [HttpPost("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginationResult<GetUsersResponse>>> GetUsers(GetUsersQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
