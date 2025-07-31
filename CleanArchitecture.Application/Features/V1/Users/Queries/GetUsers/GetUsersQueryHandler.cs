using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.SharedModels;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Features.V1.Users.Queries.LoginUser;
using CleanArchitecture.Application.Responses;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ApiResult<PaginationResult<GetUsersOutputDto>>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<ApiResult<PaginationResult<GetUsersOutputDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var (users, totalCount) = await userRepository.GetPaginatedAsync(request.Page, request.PerPage, cancellationToken);

            var dtoList = users.Select(user => new GetUsersOutputDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName
            }).ToList();

            var result = new PaginationResult<GetUsersOutputDto>
            {
                Result = dtoList,
                TotalCount = totalCount
            };

            return new ApiResult<PaginationResult<GetUsersOutputDto>>(true, ApiResultStatusCode.Success, result);
        }
    }
}
