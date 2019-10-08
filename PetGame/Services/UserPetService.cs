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

        public UserPetService(IUserPetRepository userPetRepo, IPetService petService, IUserService userService, IActionService actionService)
        {
            this.userPetRepo = userPetRepo;
            this.petService = petService;
            this.userService = userService;
            this.actionService = actionService;
        }

        /// <summary>
        /// Create a UserPet associated to the user for the selected pet.
        /// </summary>
        /// <param name="petId">Id of the Pet</param>
        /// <param name="userId">If of the User</param>
        /// <returns>Object of the created UserPet</returns>
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
        /// <summary>
        /// Get the list of all the UserPet of an user.
        /// </summary>
        /// <param name="userId">Id of an user</param>
        /// <returns>List of UserPet objects</returns>
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
        /// <summary>
        /// Updates the hunger and happiness values of a pet based on the last time it was updated and the feeding and stroking values.
        /// </summary>
        /// <param name="pet">Object of the UserPet</param>
        /// <param name="feedingValue">Int value to decrease the Hunger of the pet.</param>
        /// <param name="strokingValue">Int value to increase the Happiness of the pet.</param>
        /// <returns>Updated object of the UserPet</returns>
        public async Task<UserPet> UpdatePetStatus(UserPet pet, int feedingValue, int strokingValue)
        {
            if (pet == null)
            {
                Console.WriteLine($"{nameof(UserPetService)} - UpdatePetStatus - Pet is null.");
                return null;
            }

            var minutesSinceLastUpdate = (int)DateTimeOffset.UtcNow.Subtract(pet.LastUpdate).TotalMinutes; ;

            pet.Hunger = CalculatePetHunger(minutesSinceLastUpdate, pet.Pet.HungerRatio, feedingValue, pet.Hunger);
            pet.Happiness = CalculatePetHappiness(minutesSinceLastUpdate, pet.Pet.HappinessRatio, strokingValue,
                pet.Happiness);

            return await userPetRepo.UpdateUserPet(pet);
        }

        /// <summary>
        /// Calculates the new value of happiness.
        /// </summary>
        /// <param name="minutesSinceLastUpdate">Int value of minutes since last update</param>
        /// <param name="happinessRatio">Int value of the racio that the happiness changes</param>
        /// <param name="strokingValue">Int value to add into the happiness of the pet</param>
        /// <param name="petHappiness">Current pet happiness</param>
        /// <returns>Updated int value of happiness</returns>
        internal int CalculatePetHappiness(int minutesSinceLastUpdate, int happinessRatio, int strokingValue,
            int petHappiness)
        {
            petHappiness = petHappiness - (minutesSinceLastUpdate * happinessRatio);
            return petHappiness < 0 ? 0 + strokingValue : petHappiness + strokingValue;
        }

        /// <summary>
        /// Calculate the new value of hunger.
        /// </summary>
        /// <param name="minutesSinceLastUpdate">Int value of minutes since last update</param>
        /// <param name="hungerRatio">Int value of the racio that the hunger changes</param>
        /// <param name="feedingValue">Int value to subtract on the hunger of a pet</param>
        /// <param name="petHunger">Current pet hunger</param>
        /// <returns>Updated int value of hunger</returns>
        internal int CalculatePetHunger(int minutesSinceLastUpdate, int hungerRatio, int feedingValue, int petHunger)
        {
            petHunger = petHunger + (minutesSinceLastUpdate * hungerRatio) - feedingValue;
            return petHunger < 0 ? 0 : petHunger ;
        }
        /// <summary>
        /// Feed a pet. Saves this action on the database (Actions table) and decreases the hunger value of a UserPet.
        /// </summary>
        /// <param name="userPetId">Id of the UserPet</param>
        /// <returns>Updated object of the UserPet</returns>
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
        /// <summary>
        /// Pet a pet. Saves this action on the database (Actions table) and increases the happiness value of a UserPet.
        /// </summary>
        /// <param name="userPetId">Id of the UserPet</param>
        /// <returns>Updated object of the UserPet</returns>
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
