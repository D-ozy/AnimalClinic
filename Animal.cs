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
        public string Id { get; set; }
        public string Name { get; set; } = "";
        public byte Age { get; set; }
        public AnimalType Type { get; set; }
        public Doctor? CurrentDoctor { get; set; }
        //Добавить хозяина (owner) для большего масшабирования проекта

        public Animal() => Id = Guid.NewGuid().ToString();
    }
}
