using AnimalClinic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnimalClinic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly DB db;

        public HomeController(DB db) => this.db = db;

        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            AnimalService service = new AnimalService(db);
            List<Animal> animals = await service.GetAllAnimals();
            return new ObjectResult(animals);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetAnimal(int id)
        {
            AnimalService service = new AnimalService(db);
            Animal animal = await service.GetAnimal(id);

            if(animal == null)
            {
                return StatusCode(404);
            }

            return new ObjectResult(animal);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimal(AnimalDto userData)
        {
            if (userData == null)
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { message = "Incorrect data" });
            }
            
            AnimalService service = new AnimalService(db);
            await service.AddAnimal(userData);
            return StatusCode(201);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAnimal(int id, AnimalDto userData)
        {
            if(userData == null)
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { message = "Incorrect data" });
            }

            AnimalService service = new AnimalService(db);
            await service.UpdateAnimal(id, userData);
            return StatusCode(200);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> RemoveAnimal(int id)
        {
            AnimalService service = new AnimalService(db);
            await service.RemoveAnimal(id);
            return StatusCode(200);
        }
    }
}
