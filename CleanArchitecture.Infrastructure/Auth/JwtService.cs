using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.DTOs.V1.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Infrastructure.Auth
{
    public class JwtService : IJwtService
    {
        private readonly SiteSettings _siteSettings;

        public JwtService(IOptionsSnapshot<SiteSettings> siteSettings)
        {
            _siteSettings = siteSettings.Value;
        }

        public Task<string> GenerateToken(JwtClaimDto input)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSettings.JwtConfig.SecretKey);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);

            var encryptionKey = Encoding.UTF8.GetBytes(_siteSettings.JwtConfig.Encryptkey);
            var encryptingCredentials = new EncryptingCredentials(
                new SymmetricSecurityKey(encryptionKey),
                SecurityAlgorithms.Aes128KW,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSettings.JwtConfig.Issuer,
                Audience = _siteSettings.JwtConfig.Audience,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow.AddMinutes(_siteSettings.JwtConfig.NotBeforeMinutes),
                Expires = DateTime.UtcNow.AddMinutes(_siteSettings.JwtConfig.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(GetClaims(input))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        private IEnumerable<Claim> GetClaims(JwtClaimDto input)
        {
            var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, input.UserName),
                new Claim(ClaimTypes.NameIdentifier, input.UserId.ToString()),
                new Claim(securityStampClaimType, input.SecurityStamp)
            };

            foreach (var role in input.Role)
                claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }
    }
}
