using Microsoft.AspNetCore.Mvc;

namespace AnimalClinic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {

        private readonly static Dictionary<string, Doctor> doctors = new Dictionary<string, Doctor>()
        {
            {"Tom", new Doctor {Name = "Tom", Specialization = "Therapist", Description = "First very long description"} },
            {"John", new Doctor {Name = "John", Specialization = "Surgeon", Description = "Second very long description"} },
            {"Noah", new Doctor {Name = "Noah", Specialization = "Dermatologist", Description = "Third very long description"} }
        };

        private static List<Animal> animals = new List<Animal>()
        {
            new Animal {Name = "Murzik", Age = 3, Type = AnimalType.Cat, CurrentDoctor = doctors["Tom"]},
            new Animal {Name = "Muhtar", Age = 7, Type = AnimalType.Dog, CurrentDoctor = doctors["Noah"]}
        };

        [HttpGet]
        public IActionResult GetAllAnimals()
        {
            return new ObjectResult(animals);
        }

        [HttpGet("id")]
        public IActionResult GetAnimal(string id)
        {
            Animal? animal = animals.FirstOrDefault(a => a.Id == id);

            if (animal != null)
            {
                return new ObjectResult(animal);
            }
            else
            {
                Response.StatusCode = 404;
                return new ObjectResult(new { message = "NOT FOUND" });
            }
        }

        [HttpPost]
        public IActionResult AddAnimal(CreateAnimalDto userData)
        {
            if (userData == null)
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { message = "Incorrect data" });
            }

            Animal animal = new Animal();
            animal.CurrentDoctor = doctors["Tom"];
            animal.Name = userData.Name;
            animal.Age = userData.Age;
            animal.Type = userData.Type;

            animals.Add(animal);

            return new ObjectResult(animal);
        }

        [HttpPut]
        public IActionResult UpdateAnimal(Animal userData)
        {
            if (userData == null)
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { message = "Incorrect data" });
            }

            var animal = animals.FirstOrDefault(a => a.Id == userData.Id);

            if (animal == null)
            {
                Response.StatusCode = 404;
                return new ObjectResult(new { message = "NOT FOUND" });
            }

            animal.Name = userData.Name;
            animal.Age = userData.Age;
            animal.Type = userData.Type;

            return new ObjectResult(animal);
        }

        [HttpDelete("id")]
        public IActionResult RemoveAnimal(string id)
        {
            var animal = animals.FirstOrDefault(a => a.Id == id);

            if (animal == null)
            {
                Response.StatusCode = 404;
                return new ObjectResult(new { message = "NOT FOUND" });
            }

            animals.Remove(animal);

            return new ObjectResult(animal);
        }
    }
}
