using CleanArchitecture.Application.Contracts.Persistence;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;

        public LogoutCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _refreshTokenRepository.RevokeAllByUserIdAsync(request.UserId, cancellationToken);
            await _userRepository.UpdateSecurityStampAsync(request.UserId, Guid.NewGuid().ToString());
            return Unit.Value;
        }
    }
}
