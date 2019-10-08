using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetGame.Domain.DataConnection;
using PetGame.Domain.Entity;
using PetGame.Repositories.Interfaces;

namespace PetGame.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PetGameDbContext petGameDb;

        public UserRepository(PetGameDbContext petGameDbContext)
        {
            this.petGameDb = petGameDbContext;
        }
        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="name">String of the user name</param>
        /// <returns>Object of the user created</returns>
        public async Task<User> CreateUser(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var user = new User() {Created = DateTimeOffset.UtcNow, Name = name};

            await petGameDb.Users.AddAsync(user);
            await petGameDb.SaveChangesAsync();

            return user;
        }
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">Int id of the user</param>
        /// <returns>Object of a User</returns>
        public async Task<User> GetUser(int id)
        {
            return await petGameDb.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
