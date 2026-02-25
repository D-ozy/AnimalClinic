namespace AnimalClinic
{
    public class CreateAnimalDto
    {
        public string Name { get; set; } = "";
        public byte Age { get; set; }
        public AnimalType Type { get; set; }
    }
}
