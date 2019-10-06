using System.Collections.Generic;
using System.Threading.Tasks;
using PetGame.Domain.Entity;

namespace PetGame.Services.Interface
{
    public interface IPetService
    {
        Task<List<Pet>> GetPetList();
        Task<Pet> GetPet(int id);
    }
}
