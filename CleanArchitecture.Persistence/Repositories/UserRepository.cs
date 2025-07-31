using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
            user.SecurityStamp = Guid.NewGuid().ToString();

            return await AddAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await TableNoTracking
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPasswordHash)
        {
            var user = await GetByIdAsync(default, userId);
            if (user is null)
                return false;

            user.PasswordHash = newPasswordHash;
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.DateModified = DateTime.UtcNow;

            await UpdateAsync(user,default);
            return true;
        }

        public async Task<bool> IsEmailOrUsernameTakenAsync(string email, string userName)
        {
            return await TableNoTracking.AnyAsync(u =>
                u.Email.ToLower() == email.ToLower() ||
                u.UserName.ToLower() == userName.ToLower());
        }

        public async Task<User?> GetByUserNameAndPasswordAsync(string userName, string passwordHash)
        {
            var user = await Table
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());

            if (user is null)
                return null;

            bool isValidPassword = PasswordHasher.VerifyPassword(user.PasswordHash, passwordHash);

            return isValidPassword ? user : null;
        }

        public async Task<(List<User> Users, long TotalCount)> GetPaginatedAsync(int page, int perPage, CancellationToken cancellationToken)
        {
            var query = TableNoTracking;

            var totalCount = await query.CountAsync(cancellationToken);

            var users = await query
                .Skip(page * perPage)
                .Take(perPage)
                .ToListAsync(cancellationToken);

            return (users, totalCount);
        }

        public async Task<bool> UpdateSecurityStampAsync(int userId)
        {
            var user = await GetByIdAsync(default, userId);

            if (user is null)
                return false;

            user.SecurityStamp = Guid.NewGuid().ToString();
            user.DateModified = DateTime.UtcNow;

            await UpdateAsync(user, default);
            return true;
        }
    }
}
