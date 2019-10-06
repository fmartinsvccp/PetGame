using System.Threading.Tasks;
using PetGame.Domain.Entity;

namespace PetGame.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(string name);
        Task<User> GetUser(int id);
    }
}
