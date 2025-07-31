using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUser
{
    public class GetUserByIdQuery : IRequest<ApiResult<GetUserByIdOutputDto>>
    {
        public int Id { get; set; }
    }
}
