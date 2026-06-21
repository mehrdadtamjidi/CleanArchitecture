using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.Shared;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Domain.Entities;
using RefreshTokenEntity = CleanArchitecture.Domain.Entities.RefreshToken;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly SiteSettings _siteSettings;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtService jwtService,
            IPasswordHasher passwordHasher,
            IOptionsSnapshot<SiteSettings> siteSettings)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _siteSettings = siteSettings.Value;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.UserName);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                throw new BadRequestException("Invalid username or password.");

            var securityStamp = Guid.NewGuid().ToString();
            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

            var accessToken = await _jwtService.GenerateToken(new JwtClaimDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                SecurityStamp = securityStamp,
                Role = roles
            });

            var refreshToken = new RefreshTokenEntity
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(_siteSettings.JwtConfig.RefreshTokenExpirationDays)
            };

            await _userRepository.UpdateSecurityStampAsync(user.Id, securityStamp);
            await _refreshTokenRepository.CreateAsync(refreshToken, cancellationToken);

            return new LoginUserResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
