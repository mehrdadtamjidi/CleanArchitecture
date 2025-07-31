using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Responses;
using MediatR;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUser
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApiResult<GetUserByIdOutputDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<ApiResult<GetUserByIdOutputDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, request.Id);

            if (user == null)
            {
                return new ApiResult<GetUserByIdOutputDto>(false, ApiResultStatusCode.NotFound, null);
            }

            var getUserByIdOutputDto =  new GetUserByIdOutputDto
            {
               FirstName = user.FirstName,
               LastName = user.LastName,
               Email = user.Email,
               UserName = user.UserName
            };

            return new ApiResult<GetUserByIdOutputDto>(true, ApiResultStatusCode.Success, getUserByIdOutputDto);

        }
    }
}
