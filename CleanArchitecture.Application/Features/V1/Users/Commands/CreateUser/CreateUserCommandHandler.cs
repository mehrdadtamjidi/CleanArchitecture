using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.DTOs.V1.Users;
using CleanArchitecture.Application.Responses;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResult<CreateUserOutputDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public CreateUserCommandHandler(IUserRepository userRepository,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<ApiResult<CreateUserOutputDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            //var validator = new CreateUserCommandValidator();
            //var validationResult = await validator.ValidateAsync(request);

            //if (validationResult.IsValid == false) 
            //{
            //    if (!validationResult.IsValid)
            //    {
            //        throw new ValidationException(validationResult.Errors);
            //    }
            //}

           var aa =  userRepository.TableNoTracking.ToList();

            throw new NotImplementedException();
        }
    }
}
