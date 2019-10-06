using System;
using System.Threading.Tasks;
using Moq;
using PetGame.Domain.Entity;
using PetGame.Models;
using PetGame.Repositories.Interfaces;
using PetGame.Services;
using Xunit;

namespace PetGame.UnitTests.Services
{
    public class ActionServiceTests
    {
        private User GetUserTest()
        {
            return new User
            {
                Created = new DateTime(2019, 9, 6, 10, 10, 10),
                Id = 1,
                Name = "Goku",
                UserPets = null,
            };
        }

        private UserPet GetUserPetTest()
        {
            return new UserPet
            {
                Actions = null,
                DateOfBirth = new DateTime(2019, 9, 5, 10, 10, 10),
                Happiness = 100,
                Hunger = 0,
                Id = 1,
                LastUpdate = new DateTime(2019, 9, 6, 10, 10, 10),
                Pet = null,
                User = GetUserTest()
            };
        }


        [Fact]
        public async Task ActionService_CreateActionTest()
        {
            //Arrange 
            var actionRepoMock = new Mock<IActionRepository>();
            var actionService = new ActionService(actionRepoMock.Object);
            var actionType = ActionTypeEnum.Feed;
            var userPet = GetUserPetTest();
            var newAction = new Domain.Entity.Action
            {
                ActionType = actionType,
                UserPet = GetUserPetTest(),
                Date = new DateTime(2019, 9, 6, 10, 15, 10),
                Id = 1
            };

            actionRepoMock.Setup(repo => repo.CreateAction(userPet, actionType))
                .ReturnsAsync(newAction)
                .Verifiable();

            //Act 
            var result = await actionService.CreateAction(actionType, userPet);

            //Assert

            Assert.IsType<Domain.Entity.Action>(result);
            actionRepoMock.Verify();
            Assert.Equal(actionType, result.ActionType);
            Assert.Equal(userPet.Id, result.UserPet.Id);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task ActionService_CreateActionTest_NullParameter()
        {
            //Arrange 
            var actionRepoMock = new Mock<IActionRepository>();
            var actionService = new ActionService(actionRepoMock.Object);
            var actionType = ActionTypeEnum.Feed;

            //Act 
            var result = await actionService.CreateAction(actionType, null);

            //Assert

            Assert.Null(result);
            actionRepoMock.Verify();
        }
    }
}
