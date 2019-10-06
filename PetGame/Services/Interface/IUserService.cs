using System.Threading.Tasks;
using PetGame.Domain.Entity;

namespace PetGame.Services.Interface
{
    public interface IUserService
    {
        Task<User> CreateUser(string name);
        Task<User> GetUser(int id);
    }
}
