using AutoMapper;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, ApiResult<LoginUserOutputDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IJwtService jwtService;
        public LoginUserQueryHandler(IUserRepository userRepository, IMapper mapper, IJwtService jwtService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.jwtService = jwtService;
        }
        public async Task<ApiResult<LoginUserOutputDto>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {

            var user = await userRepository.GetByUserNameAndPasswordAsync(request.UserName, request.PasswordHash);

            if (user == null)
            {
                return new ApiResult<LoginUserOutputDto>(false,ApiResultStatusCode.UnAuthorized,null,"Invalid username or password");
            }

            
            return new ApiResult<LoginUserOutputDto>(true,ApiResultStatusCode.Success,null);

        }
    }
}
