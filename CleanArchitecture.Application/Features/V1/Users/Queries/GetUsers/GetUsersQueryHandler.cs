using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.SharedModels;
using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginationResult<GetUsersResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PaginationResult<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var (users, totalCount) = await _userRepository.GetPaginatedAsync(request.Page, request.PerPage, cancellationToken);

            var dtoList = users.Select(user => new GetUsersResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName
            }).ToList();

            return new PaginationResult<GetUsersResponse>
            {
                Result = dtoList,
                TotalCount = totalCount
            };
        }
    }
}
