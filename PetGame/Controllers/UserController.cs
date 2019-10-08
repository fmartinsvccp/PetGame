using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetGame.Services.Interface;

namespace PetGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Creates a new user for the game. 
        /// </summary>
        /// <param name="name">User name</param>
        /// <returns>User object in a Json format</returns>
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
    }
}