using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.DTOs.V1.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Auth
{
    namespace CleanArchitecture.Infrastructure.Auth
    {
        public class JwtService : IJwtService
        {
            private readonly SiteSettings siteSettings;

            public JwtService(IOptionsSnapshot<SiteSettings> siteSettings)
            {
                this.siteSettings = siteSettings.Value;
            }

            public Task<string> GenerateToken(JwtClaimDto input)
            {
                var secretKey = Encoding.UTF8.GetBytes(siteSettings.JwtConfig.SecretKey);
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature);

                var encryptionKey = Encoding.UTF8.GetBytes(siteSettings.JwtConfig.Encryptkey);
                var encryptingCredentials = new EncryptingCredentials(
                    new SymmetricSecurityKey(encryptionKey),
                    SecurityAlgorithms.Aes128KW,
                    SecurityAlgorithms.Aes128CbcHmacSha256);

                var jwtClaims = GetClaims(input);

                var descriptor = new SecurityTokenDescriptor
                {
                    Issuer = siteSettings.JwtConfig.Issuer,
                    Audience = siteSettings.JwtConfig.Audience,
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow.AddMinutes(siteSettings.JwtConfig.NotBeforeMinutes),
                    Expires = input.Remember
                        ? DateTime.UtcNow.AddDays(siteSettings.JwtConfig.ExpirationDays)
                        : DateTime.UtcNow.AddMinutes(siteSettings.JwtConfig.ExpirationMinutes),
                    SigningCredentials = signingCredentials,
                    EncryptingCredentials = encryptingCredentials,
                    Subject = new ClaimsIdentity(jwtClaims)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(descriptor);
                var jwt = tokenHandler.WriteToken(token);

                return Task.FromResult(jwt);
            }

            private IEnumerable<Claim> GetClaims(JwtClaimDto input)
            {
                var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

                var list = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, input.UserName),
                    new Claim(ClaimTypes.NameIdentifier, input.UserId.ToString()),
                    new Claim(securityStampClaimType,  input.SecurityStamp.ToString()),
                   // new Claim(ClaimTypes.Role,  input.Role.ToString())
                };

                foreach (var role in input.Role)
                    list.Add(new Claim(ClaimTypes.Role, role));

                return list;
            }
        }
    }

}
