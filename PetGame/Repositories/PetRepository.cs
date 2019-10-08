using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetGame.Domain.DataConnection;
using PetGame.Domain.Entity;
using PetGame.Repositories.Interfaces;

namespace PetGame.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly PetGameDbContext petGameDb;

        public PetRepository(PetGameDbContext petGameDbContext)
        {
            this.petGameDb = petGameDbContext;
        }
        /// <summary>
        /// Creates a new Pet
        /// </summary>
        /// <param name="name">String of the pet name</param>
        /// <param name="hungerRatio">int of the ratio that the hunger will increase</param>
        /// <param name="happinessRatio">int of the ratio that the happiness will decrease</param>
        /// <returns>Object of the created Pet</returns>
        public async Task<Pet> CreatePet(string name, int hungerRatio, int happinessRatio)
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine($"{nameof(PetRepository)} - CreatePet - Name can't be null or empty");
                return null;
            }

            var pet = new Pet {HappinessRatio = happinessRatio, HungerRatio = hungerRatio, Name = name};

            await petGameDb.Pets.AddAsync(pet);
            await petGameDb.SaveChangesAsync();

            return pet;
        }

        /// <summary>
        /// Get a pet by Id
        /// </summary>
        /// <param name="id">Id of the Pet</param>
        /// <returns>Object of the Pet</returns>
        public async Task<Pet> GetPet(int id)
        {
            return await petGameDb.Pets.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Get the list of all the pets in the game.
        /// </summary>
        /// <returns>List of pet objects</returns>
        public async Task<List<Pet>> GetPetList()
        {
            return await petGameDb.Pets.ToListAsync();
        }
    }
}
