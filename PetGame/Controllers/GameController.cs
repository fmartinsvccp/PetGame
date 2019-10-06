using System.Collections.Generic;
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
    public class GameController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IPetService petService;
        private readonly IUserPetService userPetService;

        public GameController(IUserService userService, IPetService petService, IUserPetService userPetService)
        {
            this.userService = userService;
            this.petService = petService;
            this.userPetService = userPetService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromHeader(Name = "name")] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name can't be null or empty.");
            }

            var user = await userService.CreateUser(name);

            return Ok(user);
        }

        [HttpGet("GetPetList")]
        public async Task<IActionResult> GetPetList()
        {
            var petList = await petService.GetPetList();

            return Ok(petList);
        }

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