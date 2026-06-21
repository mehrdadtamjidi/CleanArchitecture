using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.SharedModels;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Responses;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ApiResult<PaginationResult<GetUsersOutputDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResult<PaginationResult<GetUsersOutputDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var (users, totalCount) = await _userRepository.GetPaginatedAsync(request.Page, request.PerPage, cancellationToken);

            var dtoList = users.Select(user => new GetUsersOutputDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName
            }).ToList();

            return new ApiResult<PaginationResult<GetUsersOutputDto>>(true, ApiResultStatusCode.Success, new PaginationResult<GetUsersOutputDto>
            {
                Result = dtoList,
                TotalCount = totalCount
            });
        }
    }
}
