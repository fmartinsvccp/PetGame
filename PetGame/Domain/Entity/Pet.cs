using System.ComponentModel.DataAnnotations;

namespace PetGame.Domain.Entity
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int HungerRatio { get; set; }
        public int HappinessRatio { get; set; }
    }
}
