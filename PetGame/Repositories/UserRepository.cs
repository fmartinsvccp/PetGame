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

        public async Task<User> GetUser(int id)
        {
            return await petGameDb.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
