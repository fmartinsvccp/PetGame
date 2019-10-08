using System.Threading.Tasks;
using PetGame.Domain.Entity;
using PetGame.Repositories.Interfaces;
using PetGame.Services.Interface;

namespace PetGame.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepo;

        public UserService(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }
        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="name">String of the user name.</param>
        /// <returns>Object of the user.</returns>
        public async Task<User> CreateUser(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return await userRepo.CreateUser(name);
        }
        /// <summary>
        /// Get user by Id.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns>Object of the user.</returns>
        public async Task<User> GetUser(int id)
        {
            return await userRepo.GetUser(id);
        }
    }
}
