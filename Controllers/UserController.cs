using AnimalClinicLogic;
using AnimalClinicLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace AnimalClinic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "user")]
    public class UserController : Controller
    {
        private readonly DB db;
        private readonly UserService service;

        public UserController(DB db, UserService service)
        {
            this.db = db;
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;


            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var user = await service.GetUser(userId);

            if (user == null)
                return NotFound();
            

            return Ok(user);
        }

        [HttpGet("animal")]
        public async Task<IActionResult> GetAllAnimals()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;


            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var animals = await service.GetAllAnimals(userId);

            if (animals == null || animals.Count == 0)
                return new ObjectResult(new { message = "There are no animals" });


            return Ok(animals);
        }
    }
}
