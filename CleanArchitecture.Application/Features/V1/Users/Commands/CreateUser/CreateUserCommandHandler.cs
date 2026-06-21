using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserOutputDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<CreateUserOutputDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool isDuplicate = await _userRepository.IsEmailOrUsernameTakenAsync(request.Email, request.UserName);

            if (isDuplicate)
                throw new BadRequestException("Email or username already exists.");

            var user = _mapper.Map<User>(request);
            user.PasswordHash = _passwordHasher.HashPassword(request.Password);

            var createdUser = await _userRepository.CreateUserAsync(user);

            if (createdUser?.Id > 0)
                return new CreateUserOutputDto { Id = createdUser.Id };

            throw new LogicException("User could not be created.");
        }
    }
}
