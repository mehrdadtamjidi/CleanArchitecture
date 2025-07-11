using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Contracts.Persistence;
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
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResult<CreateUserOutputDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public CreateUserCommandHandler(IUserRepository userRepository,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<ApiResult<CreateUserOutputDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool isDuplicate = await userRepository.IsEmailOrUsernameTakenAsync(request.Email, request.UserName);

            if (isDuplicate)
            {
                return new ApiResult<CreateUserOutputDto>(false, ApiResultStatusCode.BadRequest, null, "Email or username already exists.");
            }

            var user = mapper.Map<User>(request);

            var createdUser = await userRepository.CreateUserAsync(user);

            if (createdUser?.Id > 0)
            {
                return new ApiResult<CreateUserOutputDto>(true, ApiResultStatusCode.Success, null);
            }

            return new ApiResult<CreateUserOutputDto>(false, ApiResultStatusCode.LogicError, null);

        }
    }
}
