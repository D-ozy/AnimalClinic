namespace AnimalClinic
{
    public enum AnimalType {
        Cat = 1,
        Dog,
        Horse,
        Cow,
        Snake,
        Bird,
        Mouse,
    }

    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public AnimalType Type { get; set; }
        public int CurrentDoctor { get; set; }
    }
}
