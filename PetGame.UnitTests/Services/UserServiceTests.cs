using System;
using System.Threading.Tasks;
using Moq;
using PetGame.Domain.Entity;
using PetGame.Repositories.Interfaces;
using PetGame.Services;
using Xunit;

namespace PetGame.UnitTests.Services
{
    public class UserServiceTests
    {
        private User GetUserTest()
        {
            return new User()
            {
                Created = new DateTime(2019, 9, 6),
                Id = 1,
                Name = "Bulma",
                UserPets = null
            };
        }

        [Fact]
        public async Task UserService_CreateUserTest()
        {
            //Arrange 
            var userRepoMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepoMock.Object);
            var user = GetUserTest();

            userRepoMock.Setup(repo => repo.CreateUser(user.Name))
                .ReturnsAsync(user)
                .Verifiable();

            //Act 
            var result = await userService.CreateUser(user.Name);

            //Assert
            Assert.IsType<User>(result);
            userRepoMock.Verify();
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task UserService_CreateUserTest_EmptyStringParameter()
        {
            //Arrange 
            var userRepoMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepoMock.Object);

            //Act 
            var result = await userService.CreateUser("");

            //Assert
            Assert.Null(result);
            userRepoMock.Verify(repo => repo.CreateUser(""), Times.Never());
        }

        [Fact]
        public async Task UserService_GetUserTest()
        {
            //Arrange 
            var userRepoMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepoMock.Object);
            var user = GetUserTest();

            userRepoMock.Setup(repo => repo.GetUser(user.Id))
                .ReturnsAsync(user)
                .Verifiable();

            //Act 
            var result = await userService.GetUser(user.Id);

            //Assert
            Assert.IsType<User>(result);
            userRepoMock.Verify();
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task UserService_GetUserTest_WrongId()
        {
            //Arrange 
            var userRepoMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepoMock.Object);
            var userId = 2;

            userRepoMock.Setup(repo => repo.GetUser(userId))
                .ReturnsAsync((User)null)
                .Verifiable();

            //Act 
            var result = await userService.GetUser(userId);

            //Assert
            Assert.Null(result);
            userRepoMock.Verify();
        }
    }
}
