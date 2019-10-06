using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetGame.Domain.DataConnection;
using PetGame.Domain.Entity;
using PetGame.Repositories.Interfaces;

namespace PetGame.Repositories
{
    public class UserPetRepository : IUserPetRepository
    {
        private readonly PetGameDbContext petGameDb;

        public UserPetRepository(PetGameDbContext petGameDbContext)
        {
            this.petGameDb = petGameDbContext;
        }

        public async Task<UserPet> CreateUserPet(User user, Pet pet)
        {
            if (user == null || pet == null)
            {
                return null;
            }

            var userPet = new UserPet
            {
                DateOfBirth = DateTimeOffset.UtcNow,
                Happiness = 100,
                Hunger = 0,
                LastUpdate = DateTimeOffset.UtcNow,
                User = user,
                Pet = pet
            };

            await petGameDb.UserPets.AddAsync(userPet);
            await petGameDb.SaveChangesAsync();

            return userPet;
        }

        public async Task<UserPet> GetUserPet(int id)
        {
            return await petGameDb.UserPets
                .Include(p => p.Pet)
                .Include(p => p.User)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserPet> UpdateUserPet(UserPet userPetModified)
        {
            if (userPetModified.User == null || userPetModified.Pet == null)
            {
                return null;
            }

            var userPet = await petGameDb.UserPets.FirstOrDefaultAsync(u => u.Id == userPetModified.Id);
            petGameDb.UserPets.Attach(userPet);
            if (userPet == null)
            {
                return null;
            }

            userPet.Happiness = userPetModified.Happiness;
            userPet.Hunger = userPetModified.Hunger;
            userPet.LastUpdate = DateTimeOffset.UtcNow;

            petGameDb.Entry(userPet).State = EntityState.Modified;
            await petGameDb.SaveChangesAsync();

            return userPet;
        }

        public async Task<List<UserPet>> GetUserPetsByUser(User user)
        {
            if (user == null)
            {
                return null;
            }

            return await petGameDb.UserPets.Where(u => u.User.Id == user.Id).Include(p => p.Pet).ToListAsync();
        }
    }
}
