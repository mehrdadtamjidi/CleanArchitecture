using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.UnitTests.Mocks;
using Moq;

namespace CleanArchitecture.Application.UnitTests.Users.Queries.GetUser
{
    public class GetUserQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _mockRepository;

        public GetUserQueryHandlerTests()
        {
            _mockRepository = MockUserRepository.GetUsersRepository();
            _mapper = Mock.Of<IMapper>();
        }

        [Fact]
        public async Task GetUserListTest()
        {

        }
    }
}
