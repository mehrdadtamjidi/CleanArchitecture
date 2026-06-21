using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.V1.Users;
using RefreshTokenEntity = CleanArchitecture.Domain.Entities.RefreshToken;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginUserResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly SiteSettings _siteSettings;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IJwtService jwtService,
            IOptionsSnapshot<SiteSettings> siteSettings)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _siteSettings = siteSettings.Value;
        }

        public async Task<LoginUserResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var existing = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (existing == null || existing.IsRevoked || existing.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedException("Refresh token is invalid or expired.");

            var user = existing.User;
            var securityStamp = Guid.NewGuid().ToString();
            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

            var newAccessToken = await _jwtService.GenerateToken(new JwtClaimDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                SecurityStamp = securityStamp,
                Role = roles
            });

            var newRefreshToken = new RefreshTokenEntity
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(_siteSettings.JwtConfig.RefreshTokenExpirationDays)
            };

            await _refreshTokenRepository.RevokeAsync(existing, cancellationToken);
            await _refreshTokenRepository.CreateAsync(newRefreshToken, cancellationToken);
            await _userRepository.UpdateSecurityStampAsync(user.Id, securityStamp);

            return new LoginUserResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}
