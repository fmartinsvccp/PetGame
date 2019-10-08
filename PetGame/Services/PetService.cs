using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PetGame.Domain.Entity;
using PetGame.Repositories.Interfaces;
using PetGame.Services.Interface;

namespace PetGame.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository petRepo;
        private readonly IMemoryCache cache;

        public PetService(IPetRepository petRepo, [FromServices]IMemoryCache cache)
        {
            this.petRepo = petRepo;
            this.cache = cache;
        }
        /// <summary>
        /// Gets all the pets.
        /// </summary>
        /// <returns>List of objects of Pets.</returns>
        public async Task<List<Pet>> GetPetList()
        {
            var data = await cache.GetOrCreateAsync("PetList", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(60);
                return await petRepo.GetPetList();
            });
            return data;
        }
        /// <summary>
        /// Gets a pet by id.
        /// </summary>
        /// <param name="id">Id of the pet.</param>
        /// <returns>Object of the Pet</returns>
        public async Task<Pet> GetPet(int id)
        {
            var data = await cache.GetOrCreateAsync(id, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                return await petRepo.GetPet(id);
            });

            return data;
        }
    }
}
