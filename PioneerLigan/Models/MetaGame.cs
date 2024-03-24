using PioneerLigan.HelperClasses;

namespace PioneerLigan.Models
{
    public class MetaGame
    {
        public int Id { get; set; }
        public LeagueEvent LeagueEvent { get; set; } = new LeagueEvent();
        public List<Deck> Decks { get; set; } = new List<Deck>();
    }
}
