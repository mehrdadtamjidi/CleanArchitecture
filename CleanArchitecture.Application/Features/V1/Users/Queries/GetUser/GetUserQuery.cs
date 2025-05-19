using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserDto>
    {
        public int Id { get; set; }
    }
}
