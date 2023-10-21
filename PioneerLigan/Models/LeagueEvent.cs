namespace PioneerLigan.Models
{
    public class LeagueEvent
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int EventNumber { get; set; }
        public int LeagueID { get; set; }
        public string MetaGame { get; set; } = string.Empty;
    }

    public class DeckInfo
    {
        public string DeckName { get; set; } = string.Empty;
        public int NoOfDecks { get; set; }
    }
}
