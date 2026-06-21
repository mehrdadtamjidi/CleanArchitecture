using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.SharedModels;
using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginationResult<GetUsersOutputDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PaginationResult<GetUsersOutputDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var (users, totalCount) = await _userRepository.GetPaginatedAsync(request.Page, request.PerPage, cancellationToken);

            var dtoList = users.Select(GetUsersOutputDto.FromEntity).ToList();

            return new PaginationResult<GetUsersOutputDto>
            {
                Result = dtoList,
                TotalCount = totalCount
            };
        }
    }
}
