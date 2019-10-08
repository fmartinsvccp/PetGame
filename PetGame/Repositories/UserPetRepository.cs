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
        /// <summary>
        /// Creates a new UserPet.
        /// </summary>
        /// <param name="user">Object of the User</param>
        /// <param name="pet">Object of the Pet</param>
        /// <returns>Object of the created UserPet</returns>
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
        /// <summary>
        /// Get a UserPet by Id.
        /// </summary>
        /// <param name="id">Int value of the UserPet</param>
        /// <returns>Object of the UserPet</returns>
        public async Task<UserPet> GetUserPet(int id)
        {
            return await petGameDb.UserPets
                .Include(p => p.Pet)
                .Include(p => p.User)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        /// <summary>
        /// Saves into the database an update of a UserPet
        /// </summary>
        /// <param name="userPetModified">Object of UserPet</param>
        /// <returns>Saved object of the UserPet</returns>
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
        /// <summary>
        /// Gets all the UserPet of an User.
        /// </summary>
        /// <param name="user">Object of an user.</param>
        /// <returns>List of UserPet objects</returns>
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
