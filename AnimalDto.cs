namespace AnimalClinic
{
    public class AnimalDto
    {
        public string Name { get; set; } = "";
        public byte Age { get; set; }
        public AnimalType Type { get; set; }
        public string DoctorName { get; set; } = "";
    }
}
