using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserOutputDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<CreateUserOutputDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool isDuplicate = await _userRepository.IsEmailOrUsernameTakenAsync(request.Email, request.UserName);

            if (isDuplicate)
                throw new BadRequestException("Email or username already exists.");

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                Gender = request.Gender,
                PasswordHash = _passwordHasher.HashPassword(request.Password)
            };

            var createdUser = await _userRepository.CreateUserAsync(user);

            if (createdUser?.Id > 0)
                return new CreateUserOutputDto { Id = createdUser.Id };

            throw new LogicException("User could not be created.");
        }
    }
}
