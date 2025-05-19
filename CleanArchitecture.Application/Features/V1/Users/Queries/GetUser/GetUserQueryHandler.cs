using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.V1.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserDto>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<GetUserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {

            var getUser = userRepository.TableNoTracking.Where(x=>x.Id == request.Id).FirstOrDefault();
            throw new NotImplementedException();
        }
    }
}
