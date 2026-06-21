using CleanArchitecture.Application.DTOs.SharedModels;
using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<PaginationResult<GetUsersOutputDto>>
    {
        public int Page { get; set; } = 0;
        public int PerPage { get; set; } = 10;

        public static GetUsersQuery FromDto(GetUsersInputDto dto) => new()
        {
            Page = dto.Page,
            PerPage = dto.PerPage
        };
    }
}
