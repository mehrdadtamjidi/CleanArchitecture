using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUser
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdOutputDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserByIdOutputDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, request.Id);

            if (user == null)
                throw new NotFoundException($"User with id {request.Id} not found.");

            return GetUserByIdOutputDto.FromEntity(user);
        }
    }
}
