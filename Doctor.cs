namespace AnimalClinic
{
    public class Doctor
    {
        public string Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Specialization { get; set; } = "";

        public Doctor() => Id = Guid.NewGuid().ToString();
    }
}
