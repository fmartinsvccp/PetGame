using System.Collections.Generic;
using System.Threading.Tasks;
using PetGame.Domain.Entity;
using PetGame.Models;

namespace PetGame.Repositories.Interfaces
{
    public interface IActionRepository
    {
        Task<Action> CreateAction(UserPet userPet, ActionTypeEnum actionType);
        Task<List<Action>> GetActionsListByUserPet(UserPet userPet);
    }
}
