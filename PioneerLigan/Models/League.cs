namespace PioneerLigan.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public string GroupStageWinner { get; set; } = string.Empty;
        public int NumberOfEvents { get; set; }
        public int NumberOfEventsToCount { get; set; }
    }
}
