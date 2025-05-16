using CleanArchitecture.Application.DTOs.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserDto>
    {
        public int Id { get; set; }
    }
}
