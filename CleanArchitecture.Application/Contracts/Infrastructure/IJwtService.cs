using CleanArchitecture.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Contracts.Infrastructure
{
    public interface IJwtService
    {
        Task<string> GenerateToken(JwtClaimDto claims);
    }
}
