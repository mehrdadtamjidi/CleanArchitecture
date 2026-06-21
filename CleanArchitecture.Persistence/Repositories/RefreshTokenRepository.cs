using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RefreshTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return refreshToken;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RefreshTokens
                .Include(x => x.User)
                    .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        }

        public async Task RevokeAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            refreshToken.IsRevoked = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RevokeAllByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _dbContext.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
                token.IsRevoked = true;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
