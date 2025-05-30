using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.CustomMapping;
using CleanArchitecture.Application.UnitTests.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.UnitTests.Users.Queries.GetUser
{
    public class GetUserQueryHandlerTests
    {
        IMapper _mapper;
        Mock<IUserRepository> _mockRepository;

        public GetUserQueryHandlerTests()
        {
            _mockRepository = MockUserRepository.GetUsersRepository();

            var mapperConfig = new MapperConfiguration(m =>
            {
               // m.AddProfile<CustomMappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();

        }

        [Fact]
        public async Task GetUserListTest()
        {
   
        }
    }
}
