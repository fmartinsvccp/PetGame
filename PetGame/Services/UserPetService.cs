using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PetGame.Domain.Entity;
using PetGame.Models;
using PetGame.Models.Configuration;
using PetGame.Repositories.Interfaces;
using PetGame.Services.Interface;

[assembly: InternalsVisibleTo("PetGame.UnitTests")]
namespace PetGame.Services
{
    public class UserPetService : IUserPetService
    {
        private readonly IUserPetRepository userPetRepo;
        private readonly IPetService petService;
        private readonly IUserService userService;
        private readonly IActionService actionService;
        private readonly ITimeProviderService timeProviderService;

        public UserPetService(IUserPetRepository userPetRepo, IPetService petService, IUserService userService, IActionService actionService, ITimeProviderService timeProviderService)
        {
            this.userPetRepo = userPetRepo;
            this.petService = petService;
            this.userService = userService;
            this.actionService = actionService;
            this.timeProviderService = timeProviderService;
        }

        public async Task<UserPet> CreateUserPet(int petId, int userId)
        {
            var user = await userService.GetUser(userId);
            var pet = await petService.GetPet(petId);

            if (user == null)
            {
                Console.WriteLine($"{nameof(UserPetService)} - CreateUserPet - User not found with Id {userId}");
                return null;
            }

            if (pet == null)
            {
                Console.WriteLine($"{nameof(UserPetService)} - CreateUserPet - Pet not found with Id {petId}");
                return null;
            }

            return await userPetRepo.CreateUserPet(user, pet);
        }

        public async Task<List<UserPet>> GetUserPetList(int userId)
        {
            var user = await userService.GetUser(userId);

            if (user == null)
            {
                Console.WriteLine($"{nameof(UserPetService)} - GetUserPetList - User not found with Id {userId}");
                return null;
            }

            var petList = await userPetRepo.GetUserPetsByUser(user);

            for (int i = 0; i < petList.Count; i++)
            {
                petList[i] = await UpdatePetStatus(petList[i], 0, 0);
            }

            return petList;
        }

        public async Task<UserPet> UpdatePetStatus(UserPet pet, int feedingValue, int strokingValue)
        {
            if (pet == null)
            {
                Console.WriteLine($"{nameof(UserPetService)} - UpdatePetStatus - Pet is null.");
                return null;
            }

            var minutesSinceLastUpdate = timeProviderService.MinutesSinceLastUpdate(pet.LastUpdate);

            pet.Hunger = CalculatePetHunger(minutesSinceLastUpdate, pet.Pet.HungerRatio, feedingValue, pet.Hunger);
            pet.Happiness = CalculatePetHappiness(minutesSinceLastUpdate, pet.Pet.HappinessRatio, strokingValue,
                pet.Happiness);

            return await userPetRepo.UpdateUserPet(pet);
        }

        internal int CalculatePetHappiness(int minutesSinceLastUpdate, int happinessRatio, int strokingValue,
            int petHappiness)
        {
            petHappiness = petHappiness - (minutesSinceLastUpdate * happinessRatio) + strokingValue;
            return petHappiness < 0 ? 0 : petHappiness;
        }

        internal int CalculatePetHunger(int minutesSinceLastUpdate, int hungerRatio, int feedingValue, int petHunger)
        {
            petHunger = petHunger + (minutesSinceLastUpdate * hungerRatio) - feedingValue;
            return petHunger < 0 ? 0 : petHunger;
        }

        public async Task<UserPet> FeedPet(int userPetId)
        {
            var userPet = await userPetRepo.GetUserPet(userPetId);

            if (userPet == null)
            {
                Console.WriteLine($"{nameof(UserPetService)} - FeedPet - User Pet is null.");
                return null;
            }

            await actionService.CreateAction(ActionTypeEnum.Feed, userPet);

            return await UpdatePetStatus(userPet, Configuration.FeedingValue, 0);
        }

        public async Task<UserPet> PetPet(int userPetId)
        {
            var userPet = await userPetRepo.GetUserPet(userPetId);

            if (userPet == null)
            {
                Console.WriteLine($"{nameof(UserPetService)} - FeedPet - User Pet is null.");
                return null;
            }

            await actionService.CreateAction(ActionTypeEnum.Pet, userPet);

            return await UpdatePetStatus(userPet, 0, Configuration.PettingValue);
        }
    }
}
