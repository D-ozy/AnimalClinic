using AnimalClinic.DTO;
using AnimalClinic.Model;
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
        private readonly AnimalService service;

        public HomeController(DB db, AnimalService service)
        {
            this.db = db;
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            List<Animal> animals = await service.GetAllAnimals();
            return new ObjectResult(animals);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetAnimal(int id)
        {
            Animal animal = await service.GetAnimal(id);

            if(animal == null)
            {
                return StatusCode(404);
            }

            return new ObjectResult(animal);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAnimals(string? name, AnimalType? type, int? age)
        {
            List<Animal> animals = await service.SearchAnimals(name, type, age);

            if (animals == null || animals.Count == 0)
                return StatusCode(400);

            return new ObjectResult(animals);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimal(AnimalDto userData)
        {
            if (userData == null)
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { message = "Incorrect data" });
            }
            
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

            await service.UpdateAnimal(id, userData);
            return StatusCode(200);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> RemoveAnimal(int id)
        {
            await service.RemoveAnimal(id);
            return StatusCode(200);
        }
    }
}
