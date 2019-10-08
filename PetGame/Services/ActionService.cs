using System;
using System.Threading.Tasks;
using PetGame.Domain.Entity;
using PetGame.Models;
using PetGame.Repositories.Interfaces;
using PetGame.Services.Interface;

namespace PetGame.Services
{
    public class ActionService : IActionService
    {
        private readonly IActionRepository actionRepo;

        public ActionService(IActionRepository actionRepo)
        {
            this.actionRepo = actionRepo;
        }
        /// <summary>
        /// Creates an action.
        /// </summary>
        /// <param name="actionType">Action Type</param>
        /// <param name="userPet">Object of the UserPet</param>
        /// <returns>Object of the created Action</returns>
        public async Task<Domain.Entity.Action> CreateAction(ActionTypeEnum actionType, UserPet userPet)
        {
            if (userPet == null)
            {
                Console.WriteLine($"{nameof(ActionService)} - CreateAction - User Pet is null");
                return null;
            }
                
            return await actionRepo.CreateAction(userPet, actionType);

        }
    }
}
