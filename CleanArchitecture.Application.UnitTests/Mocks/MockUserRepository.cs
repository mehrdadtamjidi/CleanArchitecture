using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.UnitTests.Mocks
{
    public static class MockUserRepository
    {
        public static Mock<IUserRepository> GetUsersRepository()
        {
            var users = new List<User>()
            {
                new User() { Id = 1,FirstName = "",LastName = "" },
                new User() { Id = 1,FirstName = "",LastName = "" }
            };

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.TableNoTracking.ToList()).Returns(users);

            mockRepo.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                .ReturnsAsync((User user, CancellationToken token, bool saveNow) =>
                {
                    users.Add(user);
                    return user;
                });

            return mockRepo;
        }
    }
}
