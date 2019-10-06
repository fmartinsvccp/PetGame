using System.Collections.Generic;
using System.Threading.Tasks;
using PetGame.Domain.Entity;

namespace PetGame.Repositories.Interfaces
{
    public interface IPetRepository
    {
        Task<Pet> CreatePet(string name, int hungerRatio, int happinessRatio);
        Task<Pet> GetPet(int id);
        Task<List<Pet>> GetPetList();
    }
}
