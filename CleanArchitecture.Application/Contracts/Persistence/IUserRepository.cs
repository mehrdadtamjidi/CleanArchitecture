using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Contracts.Persistence
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> CreateUserAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ChangePasswordAsync(int userId, string newPasswordHash);
        Task<bool> IsEmailOrUsernameTakenAsync(string email, string userName);
        Task<User?> GetByUserNameAndPasswordAsync(string userName, string passwordHash);
        Task<(List<User> Users, long TotalCount)> GetPaginatedAsync(int page, int perPage, CancellationToken cancellationToken);
        Task<bool> UpdateSecurityStampAsync(int userId,string securityStamp);
    }
}
