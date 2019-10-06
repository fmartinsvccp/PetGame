using System;
using System.ComponentModel.DataAnnotations;
using PetGame.Models;

namespace PetGame.Domain.Entity
{
    public class Action
    {
        [Key]
        public int Id { get; set; }
        public UserPet UserPet { get; set; }
        public ActionTypeEnum ActionType { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
