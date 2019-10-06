using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using PetGame.Domain.Entity;
using PetGame.Models;
using PetGame.Repositories.Interfaces;
using PetGame.Services;
using Xunit;

namespace PetGame.UnitTests.Services
{
    public class PetServiceTests
    {
        private Pet GetPetTest()
        {
            return new Pet
            {
                HappinessRatio = 10,
                HungerRatio = 5,
                Id = 1,
                Name = "Bubu"
            };
        }

        private List<Pet> GetPetListTest()
        {
            var list = new List<Pet>();
            list.Add(new Pet
            {
                HappinessRatio = 10,
                HungerRatio = 5,
                Id = 1,
                Name = "Vegeta"
            });
            list.Add(new Pet
            {
                HappinessRatio = 10,
                HungerRatio = 5,
                Id = 1,
                Name = "Trunks"
            });
            list.Add(new Pet
            {
                HappinessRatio = 10,
                HungerRatio = 5,
                Id = 1,
                Name = "Gohan"
            });

            return list;
        }

        [Fact]
        public async Task PetService_GetPetTest()
        {
            //Arrange 
            var petId = 1;
            var petRepoMock = new Mock<IPetRepository>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var petService = new PetService(petRepoMock.Object, cache);
            var pet = GetPetTest();

            petRepoMock.Setup(repo => repo.GetPet(petId))
                .ReturnsAsync(pet)
                .Verifiable();

            //Act 
            var result = await petService.GetPet(petId);

            //Assert

            Assert.IsType<Pet>(result);
            petRepoMock.Verify();
            Assert.Equal(pet, result);
        }

        [Fact]
        public async Task PetService_GetPetTest_NonExistingId()
        {
            //Arrange 
            var petId = 2;
            var petRepoMock = new Mock<IPetRepository>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var petService = new PetService(petRepoMock.Object, cache);
            var pet = GetPetTest();

            petRepoMock.Setup(repo => repo.GetPet(petId))
                .ReturnsAsync((Pet)null)
                .Verifiable();

            //Act 
            var result = await petService.GetPet(petId);

            //Assert

            Assert.Null(result);
            petRepoMock.Verify();
        }

        [Fact]
        public async Task PetService_GetListTest()
        {
            //Arrange
            var petRepoMock = new Mock<IPetRepository>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var petService = new PetService(petRepoMock.Object, cache);
            var list = GetPetListTest();

            petRepoMock.Setup(repo => repo.GetPetList())
                .ReturnsAsync(list)
                .Verifiable();

            //Act
            var result = await petService.GetPetList();

            Assert.IsType<List<Pet>>(result);
            petRepoMock.Verify();
            Assert.Equal(list, result);
        }
    }
}
