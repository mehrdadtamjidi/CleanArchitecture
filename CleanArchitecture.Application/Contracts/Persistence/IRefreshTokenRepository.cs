using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Contracts.Persistence
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
        Task RevokeAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    }
}
