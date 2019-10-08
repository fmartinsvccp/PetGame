using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PetGame.Domain.Entity;
using PetGame.Models;
using PetGame.Repositories.Interfaces;
using PetGame.Services;
using PetGame.Services.Interface;
using Xunit;

namespace PetGame.UnitTests.Services
{
    public class UserPetServiceTests
    {
        #region Test Data
        private Pet GetPetTest()
        {
            return new Pet
            {
                HappinessRatio = 10,
                HungerRatio = 5,
                Id = 1,
                Name = "Freeza"
            };
        }

        private User GetUserTest()
        {
            return new User()
            {
                Created = new DateTime(2019, 9, 6),
                Id = 1,
                Name = "Cell",
                UserPets = null
            };
        }

        private UserPet GetUserPetTest()
        {
            return new UserPet
            {
                User = GetUserTest(),
                Actions = null,
                DateOfBirth = new DateTime(2019, 9, 6),
                Happiness = 100,
                Hunger = 0,
                Id = 1,
                LastUpdate = new DateTime(2019, 9, 6),
                Pet = GetPetTest(),
            };
        }

        private List<UserPet> GetUserPetListTest()
        {
            var list = new List<UserPet>
            {
                new UserPet
                {
                    User = GetUserTest(),
                    Actions = null,
                    DateOfBirth = new DateTime(2019, 9, 5),
                    Happiness = 100,
                    Hunger = 0,
                    Id = 1,
                    LastUpdate = new DateTime(2019, 9, 5),
                    Pet = GetPetTest(),
                },
                new UserPet
                {
                    User = GetUserTest(),
                    Actions = null,
                    DateOfBirth = new DateTime(2019, 9, 4),
                    Happiness = 100,
                    Hunger = 0,
                    Id = 2,
                    LastUpdate = new DateTime(2019, 9, 4),
                    Pet = GetPetTest(),
                }
            };

            return list;
        }
        #endregion

        [Fact]
        public async Task UserPetService_CreateUserPetTest()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);
            var userPet = GetUserPetTest();
            var user = GetUserTest();
            var pet = GetPetTest();

            userPetRepoMock.Setup(repo => repo.CreateUserPet(user, pet))
                .ReturnsAsync(userPet)
                .Verifiable();

            petServiceMock.Setup(service => service.GetPet(pet.Id))
                .ReturnsAsync(pet)
                .Verifiable();

            userServiceMock.Setup(service => service.GetUser(user.Id))
                .ReturnsAsync(user)
                .Verifiable();

            //Act 
            var result = await userPetService.CreateUserPet(pet.Id, user.Id);

            //Assert
            Assert.IsType<UserPet>(result);
            userPetRepoMock.Verify();
            petServiceMock.Verify();
            userServiceMock.Verify();
            Assert.Equal(userPet, result);
            Assert.Equal(user.Id, result.User.Id);
            Assert.Equal(pet.Id, result.Pet.Id);
        }

        [Fact]
        public async Task UserPetService_CreateUserPetTest_NullUser()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);
            var pet = GetPetTest();

            userPetRepoMock.Setup(repo => repo.CreateUserPet(null, pet))
                .ReturnsAsync((UserPet)null)
                .Verifiable();

            petServiceMock.Setup(service => service.GetPet(pet.Id))
                .ReturnsAsync(pet)
                .Verifiable();

            userServiceMock.Setup(service => service.GetUser(2))
                .ReturnsAsync((User)null)
                .Verifiable();

            //Act 
            var result = await userPetService.CreateUserPet(pet.Id, 2);

            //Assert
            Assert.Null(result);
            userPetRepoMock.Verify(repo => repo.CreateUserPet(null, pet), Times.Never());
            petServiceMock.Verify();
            userServiceMock.Verify();
        }

        [Fact]
        public async Task UserPetService_CreateUserPetTest_NullPet()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);
            var user = GetUserTest();

            userPetRepoMock.Setup(repo => repo.CreateUserPet(user, null))
                .ReturnsAsync((UserPet)null)
                .Verifiable();

            petServiceMock.Setup(service => service.GetPet(2))
                .ReturnsAsync((Pet)null)
                .Verifiable();

            userServiceMock.Setup(service => service.GetUser(user.Id))
                .ReturnsAsync(user)
                .Verifiable();

            //Act 
            var result = await userPetService.CreateUserPet(2, user.Id);

            //Assert
            Assert.Null(result);
            userPetRepoMock.Verify(repo => repo.CreateUserPet(user, null), Times.Never());
            petServiceMock.Verify();
            userServiceMock.Verify();
        }

        [Fact]
        public async Task UserPetService_GetUserPetList()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);
            var user = GetUserTest();
            var list = GetUserPetListTest();

            userPetRepoMock.Setup(repo => repo.GetUserPetsByUser(user))
                .ReturnsAsync(list)
                .Verifiable();

            userServiceMock.Setup(service => service.GetUser(user.Id))
                .ReturnsAsync(user)
                .Verifiable();

            //Act
            var result = await userPetService.GetUserPetList(user.Id);

            //Assert
            Assert.IsType<List<UserPet>>(result);
            userPetRepoMock.Verify();
            userServiceMock.Verify();
            Assert.Equal(list.Count, result.Count);
        }

        [Fact]
        public async Task UserPetService_GetUserPetList_NullUser()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);
            var user = GetUserTest();
            var list = GetUserPetListTest();

            userPetRepoMock.Setup(repo => repo.GetUserPetsByUser(user))
                .ReturnsAsync(list)
                .Verifiable();

            userServiceMock.Setup(service => service.GetUser(2))
                .ReturnsAsync((User)null)
                .Verifiable();

            //Act
            var result = await userPetService.GetUserPetList(2);

            //Assert
            Assert.Null(result);
            userPetRepoMock.Verify(repo => repo.GetUserPetsByUser(It.IsAny<User>()), Times.Never());
            userServiceMock.Verify();
        }

        [Fact]
        public async Task UserPetService_UpdatePetStatus()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);
            var pet = GetPetTest();
            var userPet = GetUserPetTest();

            userPetRepoMock.Setup(repo => repo.UpdateUserPet(userPet))
                .ReturnsAsync(userPet)
                .Verifiable();

            //Act
            var result = await userPetService.UpdatePetStatus(userPet, 0, 0);

            //Assert
            Assert.IsType<UserPet>(result);
            userPetRepoMock.Verify();
        }

        [Fact]
        public async Task UserPetService_UpdatePetStatus_UserPetIsNull()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);

            //Act
            var result = await userPetService.UpdatePetStatus(null, 0, 0);

            //Assert
            Assert.Null(result);
            userPetRepoMock.Verify(repo => repo.UpdateUserPet(It.IsAny<UserPet>()), Times.Never());
            userPetRepoMock.Verify();
        }

        [Theory]
        [InlineData(10,1,0,50,40)]
        [InlineData(15, 2, 40, 30, 40)]
        [InlineData(20, 3, 0, 30, 0)]
        public void UserPetService_CalculatePetHappiness(int minutes, int happinessRatio, int strokingValue,
            int petHappiness, int expectedResult)
        {
            //Arrange
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);

            //Act 
            var result = userPetService.CalculatePetHappiness(minutes, happinessRatio, strokingValue, petHappiness);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(10, 1, 0, 50, 60)]
        [InlineData(15, 2, 40, 30, 20)]
        [InlineData(10, 1, 100, 30, 0)]
        public void UserPetService_CalculatePetHunger(int minutes, int hungerRatio, int feedingValue,
            int petHunger, int expectedResult)
        {
            //Arrange
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);

            //Act 
            var result = userPetService.CalculatePetHunger(minutes, hungerRatio, feedingValue, petHunger);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task UserPetService_FeedPet()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);
            var userPet = GetUserPetTest();
            var actionType = ActionTypeEnum.Feed;
            var action = new Domain.Entity.Action
                {ActionType = actionType, Date = new DateTime(2019, 9, 5), Id = 1, UserPet = userPet};

            userPetRepoMock.Setup(repo => repo.GetUserPet(userPet.Id))
                .ReturnsAsync(userPet)
                .Verifiable();

            userPetRepoMock.Setup(repo => repo.UpdateUserPet(userPet))
                .ReturnsAsync(userPet)
                .Verifiable();

            actionServiceMock.Setup(service => service.CreateAction(actionType, userPet))
                .ReturnsAsync(action)
                .Verifiable();

            //Act
            var result = await userPetService.FeedPet(userPet.Id);

            Assert.IsType<UserPet>(result);
            actionServiceMock.Verify();
            userPetRepoMock.Verify();
        }

        [Fact]
        public async Task UserPetService_FeedPet_NullUserPet()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);

            userPetRepoMock.Setup(repo => repo.GetUserPet(2))
                .ReturnsAsync((UserPet)null)
                .Verifiable();

            //Act
            var result = await userPetService.FeedPet(2);

            Assert.Null(result);
            actionServiceMock.Verify(service => service.CreateAction(ActionTypeEnum.Feed, It.IsAny<UserPet>()),Times.Never());
            userPetRepoMock.Verify();
        }

        [Fact]
        public async Task UserPetService_PetPet()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);
            var userPet = GetUserPetTest();
            var actionType = ActionTypeEnum.Pet;
            var action = new Domain.Entity.Action
            { ActionType = actionType, Date = new DateTime(2019, 9, 5), Id = 1, UserPet = userPet };

            userPetRepoMock.Setup(repo => repo.GetUserPet(userPet.Id))
                .ReturnsAsync(userPet)
                .Verifiable();

            userPetRepoMock.Setup(repo => repo.UpdateUserPet(userPet))
                .ReturnsAsync(userPet)
                .Verifiable();

            actionServiceMock.Setup(service => service.CreateAction(actionType, userPet))
                .ReturnsAsync(action)
                .Verifiable();

            //Act
            var result = await userPetService.PetPet(userPet.Id);

            Assert.IsType<UserPet>(result);
            actionServiceMock.Verify();
            userPetRepoMock.Verify();
        }

        [Fact]
        public async Task UserPetService_PetPet_NullUserPet()
        {
            //Arrange 
            var userPetRepoMock = new Mock<IUserPetRepository>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var actionServiceMock = new Mock<IActionService>();
            var userPetService = new UserPetService(userPetRepoMock.Object, petServiceMock.Object, userServiceMock.Object, actionServiceMock.Object);

            userPetRepoMock.Setup(repo => repo.GetUserPet(2))
                .ReturnsAsync((UserPet)null)
                .Verifiable();

            //Act
            var result = await userPetService.PetPet(2);

            Assert.Null(result);
            actionServiceMock.Verify(service => service.CreateAction(ActionTypeEnum.Pet, It.IsAny<UserPet>()), Times.Never());
            userPetRepoMock.Verify();
        }
    }
}
