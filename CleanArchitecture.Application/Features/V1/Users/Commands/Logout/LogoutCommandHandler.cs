using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Responses;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResult>
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

        public async Task<ApiResult> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _refreshTokenRepository.RevokeAllByUserIdAsync(request.UserId, cancellationToken);
            await _userRepository.UpdateSecurityStampAsync(request.UserId, Guid.NewGuid().ToString());

            return new ApiResult(true, ApiResultStatusCode.Success);
        }
    }
}
