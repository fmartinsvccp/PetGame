using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace PetGame.Domain.Entity
{
    public class User
    {
        [Key] 
        public int Id { get; set; }
        public string Name { get; set; }
        [Index] 
        public DateTimeOffset Created { get; set; }
        public virtual ICollection<UserPet> UserPets { get; set; }
    }
}
