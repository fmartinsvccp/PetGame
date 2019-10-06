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

        public async Task<Pet> GetPet(int id)
        {
            return await petGameDb.Pets.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Pet>> GetPetList()
        {
            return await petGameDb.Pets.ToListAsync();
        }
    }
}
