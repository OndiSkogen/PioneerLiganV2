namespace PioneerLigan.ViewModels
{
    public class _LeagueEventVM
    {
        public DateTime Date { get; set; }
        public int LeagueID { get; set; }
        public int EventNumber { get; set; }
        public List<Models.EventResult> Results { get; set; } = new List<Models.EventResult>();
        public string cssId { get; set; } = string.Empty;
    }
}
