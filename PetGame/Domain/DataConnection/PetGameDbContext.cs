using Microsoft.EntityFrameworkCore;
using PetGame.Domain.Entity;

namespace PetGame.Domain.DataConnection
{
    public class PetGameDbContext : DbContext
    {
        public PetGameDbContext(DbContextOptions<PetGameDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<UserPet> UserPets { get; set; } 
        public DbSet<Action> Actions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dog = new Pet {Id = 1,  HappinessRatio = 2, HungerRatio = 3, Name = "Dog" };
            var cat = new Pet {Id = 2,  HappinessRatio = 3, HungerRatio = 2, Name = "Cat" };
            var bird = new Pet {Id = 3, HappinessRatio = 1, HungerRatio = 1, Name = "Bird" };

            modelBuilder.Entity<Pet>().HasData(dog, cat, bird);

            base.OnModelCreating(modelBuilder);
        }
    }
}
