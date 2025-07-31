using CleanArchitecture.Application.DTOs.SharedModels;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<ApiResult<PaginationResult<GetUsersOutputDto>>>
    {
        public int Page { get; set; } = 0;
        public int PerPage { get; set; } = 10;
    }
}
