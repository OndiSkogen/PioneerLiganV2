namespace PioneerLigan.Models
{
    public class EventResult
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int PlayerId { get; set; }
        public int Points { get; set; }
        public float OMW { get; set; }
        public float GW { get; set; }
        public float OGW { get; set; }
        public int Placement { get; set; }
        public string PlayerName { get; set; } = String.Empty;
    }
}
