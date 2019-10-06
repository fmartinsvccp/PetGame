using System.Threading.Tasks;
using PetGame.Domain.Entity;
using PetGame.Models;

namespace PetGame.Services.Interface
{
    public interface IActionService
    {
        Task<Action> CreateAction(ActionTypeEnum actionType, UserPet userPet);
    }
}
