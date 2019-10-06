using System.Collections.Generic;
using System.Threading.Tasks;
using PetGame.Domain.Entity;

namespace PetGame.Services.Interface
{
    public interface IUserPetService
    {
        Task<UserPet> CreateUserPet(int petId, int userId);
        Task<List<UserPet>> GetUserPetList(int userId);
        Task<UserPet> UpdatePetStatus(UserPet pet, int feedingValue, int strokingValue);
        Task<UserPet> FeedPet(int userPetId);
        Task<UserPet> PetPet(int userPetId);
    }
}
