using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Xml.Linq;

namespace AnimalClinic.Services
{
    public class AnimalService
    {
        private readonly DB db;

        public AnimalService(DB db)
        {
            this.db = db;
        }

        public async Task<List<Animal>> GetAllAnimals()
        {
            List<Animal> animals = new List<Animal>();

            await using var con = new NpgsqlConnection(db.GetConnectionstring());
            await con.OpenAsync();

            using(NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM animals", con))
            {
                using(NpgsqlDataReader reader =  cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        animals.Add(new Animal 
                        { 
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader["name"].ToString(),
                            Age = Convert.ToInt32(reader["age"]),
                            Type = Enum.Parse<AnimalType>(reader["type"].ToString()),
                            CurrentDoctor = Convert.ToInt32(reader["doctor_id"])
                        });
                    }
                }
            }

            return animals;
        }

        public async Task<Animal> GetAnimal(int id)
        {
            await using var con = new NpgsqlConnection(db.GetConnectionstring());
            Animal animal = new Animal();
            await con.OpenAsync();

            using(NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM animals WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("id", id);
                
                using(NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        animal.Id = Convert.ToInt32(reader["id"]);
                        animal.Name = reader["name"].ToString();
                        animal.Age = Convert.ToInt32(reader["age"]);
                        animal.Type = Enum.Parse<AnimalType>(reader["type"].ToString());
                        animal.CurrentDoctor = Convert.ToInt32(reader["doctor_id"]);
                    }
                    else
                        animal = null;
                }
            }

            return animal;
        }

        public async Task AddAnimal(AnimalDto animal)
        {
            await using var con = new NpgsqlConnection(db.GetConnectionstring());
            await con.OpenAsync();

            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.CommandText = $"INSERT INTO animals (name, age, type, doctor_id) VALUES (@name, @age, @type, (SELECT id FROM doctors WHERE name = @doctor_name))";
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("name", animal.Name);
                cmd.Parameters.AddWithValue("age", animal.Age);
                cmd.Parameters.AddWithValue("type", animal.Type.ToString());
                cmd.Parameters.AddWithValue("doctor_name", animal.DoctorName);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        

        public async Task UpdateAnimal(int id, AnimalDto animal)
        {
            await using var con = new NpgsqlConnection(db.GetConnectionstring());
            await con.OpenAsync();

            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.CommandText = "UPDATE animals SET name = @name, age = @age, type = @type, doctor_id = (SELECT id FROM doctors WHERE name = @doctor_name) WHERE id = @id";
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("name", animal.Name);
                cmd.Parameters.AddWithValue("age", animal.Age);
                cmd.Parameters.AddWithValue("type", animal.Type.ToString());
                cmd.Parameters.AddWithValue("doctor_name", animal.DoctorName);

                await cmd.ExecuteNonQueryAsync();

            }
        }

        public async Task RemoveAnimal(int id)
        {
            await using var con = new NpgsqlConnection(db.GetConnectionstring());
            await con.OpenAsync();

            using(NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM animals WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("id", id);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
