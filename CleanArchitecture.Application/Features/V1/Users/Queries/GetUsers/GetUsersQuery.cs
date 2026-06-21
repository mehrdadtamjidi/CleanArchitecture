using CleanArchitecture.Application.DTOs.SharedModels;
using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<PaginationResult<GetUsersResponse>>
    {
        public int Page { get; set; } = 0;
        public int PerPage { get; set; } = 10;
    }
}
