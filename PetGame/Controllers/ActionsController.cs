using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetGame.Domain.Entity;
using PetGame.Models;
using PetGame.Services.Interface;

namespace PetGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActionsController : Controller
    {
        private readonly IUserPetService userPetService;

        public ActionsController(IUserPetService userPetService)
        {
            this.userPetService = userPetService;
        }

        /// <summary>
        /// Feed a pet of an user.
        /// </summary>
        /// <param name="userPetId">Id of the UserPet</param>
        /// <returns>UserPet object with updated hunger value</returns>
        [HttpPost("FeedAnimal")]
        public async Task<IActionResult> FeedAnimal([FromHeader(Name = "userPetId")] int userPetId)
        {
            var userPet = await userPetService.FeedPet(userPetId);

            if (userPet == null)
            {
                return BadRequest("User Pet not found.");
            }

            return Ok(CreateUserPetModel(userPet));
        }

        /// <summary>
        /// Pet a pet of an user.
        /// </summary>
        /// <param name="userPetId">Id of the UserPet</param>
        /// <returns>UserPet object with updated happiness value</returns>
        [HttpPost("PetAnimal")]
        public async Task<IActionResult> PetAnimal([FromHeader(Name = "userPetId")] int userPetId)
        {
            var userPet = await userPetService.PetPet(userPetId);

            if (userPet == null)
            {
                return BadRequest("User Pet not found.");
            }

            return Ok(CreateUserPetModel(userPet));
        }

        private UserPetModel CreateUserPetModel(UserPet userPet)
        {
            return new UserPetModel
            {
                Id = userPet.Id,
                AnimalName = userPet.Pet.Name,
                DateOfBirth = userPet.DateOfBirth,
                Happiness = userPet.Happiness,
                Hunger = userPet.Hunger
            };
        }
    }
}