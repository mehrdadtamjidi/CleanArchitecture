using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, LoginUserOutputDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;
        private readonly SiteSettings _siteSettings;

        public LoginUserQueryHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtService jwtService,
            IOptionsSnapshot<SiteSettings> siteSettings)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
            _siteSettings = siteSettings.Value;
        }

        public async Task<LoginUserOutputDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameAndPasswordAsync(request.UserName, request.PasswordHash);

            if (user == null)
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

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(_siteSettings.JwtConfig.RefreshTokenExpirationDays)
            };

            await _userRepository.UpdateSecurityStampAsync(user.Id, securityStamp);
            await _refreshTokenRepository.CreateAsync(refreshToken, cancellationToken);

            return new LoginUserOutputDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
