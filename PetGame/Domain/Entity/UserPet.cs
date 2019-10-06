using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetGame.Domain.Entity
{
    public class UserPet
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public Pet Pet { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public int Hunger { get; set; }
        public int Happiness { get; set; }
        public DateTimeOffset LastUpdate { get; set; }

        public virtual ICollection<Action> Actions { get; set; }
    }
}
