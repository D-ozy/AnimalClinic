using Npgsql;
using System.Threading.Tasks;

namespace AnimalClinic
{
    public class DB
    {
        private readonly string connection = "Host=localhost;Port=5432;Database=animal_clinic_db;Username=Dozy;Password=113389Dozy";

        public string GetConnectionstring() => connection;
    }
}
