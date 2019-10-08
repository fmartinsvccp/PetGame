using System;

namespace PetGame.Models
{
    public class UserPetModel
    {
        public int Id { get; set; }
        public string AnimalName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public int Hunger { get; set; }
        public int Happiness { get; set; }
    }
}
