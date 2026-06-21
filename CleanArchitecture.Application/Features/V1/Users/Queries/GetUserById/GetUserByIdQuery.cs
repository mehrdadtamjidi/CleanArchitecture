using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<GetUserByIdResponse>
    {
        public int Id { get; set; }
    }
}
