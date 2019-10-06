using System.Collections.Generic;
using System.Threading.Tasks;
using PetGame.Domain.Entity;

namespace PetGame.Repositories.Interfaces
{
    public interface IUserPetRepository
    {
        Task<UserPet> CreateUserPet(User user, Pet pet);
        Task<UserPet> GetUserPet(int id);
        Task<UserPet> UpdateUserPet(UserPet userPetModified);
        Task<List<UserPet>> GetUserPetsByUser(User user);
    }
}
