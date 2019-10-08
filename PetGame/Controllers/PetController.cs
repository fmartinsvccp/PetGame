using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetGame.Domain.Entity;
using PetGame.Models;
using PetGame.Services.Interface;

namespace PetGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly IPetService petService;
        private readonly IUserPetService userPetService;

        public PetController(IPetService petService, IUserPetService userPetService)
        {
            this.petService = petService;
            this.userPetService = userPetService;
        }

        /// <summary>
        /// Get the list of all the pets types available.
        /// </summary>
        /// <returns>An array with all the pets objects.</returns>
        [HttpGet("GetPetList")]
        public async Task<IActionResult> GetPetList()
        {
            var petList = await petService.GetPetList();

            return Ok(petList);
        }

        /// <summary>
        /// Create a pet for the user.
        /// </summary>
        /// <param name="petId">The Id of the pet.</param>
        /// <param name="userId">The Id of the user.</param>
        /// <returns>Returns an object of the acquired pet in Json format.</returns>
        [HttpPost("GetPetForUser")]
        public async Task<IActionResult> GetPetForUser([FromHeader(Name = "petId")] int petId,
            [FromHeader(Name = "userId")] int userId)
        {
            var userPet = await userPetService.CreateUserPet(petId, userId);

            if (userPet == null)
            {
                return BadRequest("User or Pet not found.");
            }

            return Ok(CreateUserPetModel(userPet));
        }

        /// <summary>
        /// Get the list of all the pets for a specific user.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <returns>An array with all the pets objects in Json format.</returns>
        [HttpGet("GetUserPetList/{userId}")]
        public async Task<IActionResult> GetUserPetList(int userId)
        {
            var userPetList = await userPetService.GetUserPetList(userId);

            if (userPetList == null)
            {
                return BadRequest("User not found.");
            }

            var list = userPetList.Select(u => new UserPetModel
            {
                AnimalName = u.Pet.Name,
                DateOfBirth = u.DateOfBirth,
                Id = u.Id,
                Hunger = u.Hunger,
                Happiness = u.Happiness
            }).ToList();

            return Ok(list);
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