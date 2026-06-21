using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUser
{
    public class GetUserByIdQuery : IRequest<GetUserByIdOutputDto>
    {
        public int Id { get; set; }
    }
}
