namespace AnimalClinic.Contracts
{
    public class AnimalCreatedEvent
    {
        public Guid EventId { get; set; }
        public Guid IdempotencyKey { get; set; }
        public int AnimalId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
