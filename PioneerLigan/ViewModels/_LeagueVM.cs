using PioneerLigan.Models;

namespace PioneerLigan.ViewModels
{
    public class _LeagueVM
    {
        public string Name { get; set; } = string.Empty;
        public List<LeagueEvent> Events { get; set; } = new List<LeagueEvent>();
        public List<Player> Players { get; set; } = new List<Player>();
        public List<EventResult> Results { get; set; } = new List<EventResult>();
        public List<_PlayerVM> PlayersVMs { get; set; } = new List<_PlayerVM>();
        public List<_LeagueEventVM> LeagueEventVMs { get; set; } = new List<_LeagueEventVM>();
        public string GroupStageWinner { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public int EventsToCount { get; set; } = 0;

        public _LeagueVM(List<LeagueEvent> events, List<Player> players, List<EventResult> eventResults, League league)
        {
            Name = league.Name;
            GroupStageWinner = league.GroupStageWinner;
            Winner = league.Winner;
            Events = events.Where(i => i.LeagueID == league.Id).OrderBy(ev => ev.EventNumber).ToList();
            Players = players.ToList();
            EventsToCount = league.NumberOfEventsToCount;

            foreach (var ev in Events)
            {
                var tempResults = new List<EventResult>();

                tempResults.AddRange(eventResults.Where(i => i.EventId == ev.Id).OrderBy(p => p.Placement));
                Results.AddRange(eventResults.Where(i => i.EventId == ev.Id).OrderBy(p => p.Placement));

                LeagueEventVMs.Add(new _LeagueEventVM { Date = ev.Date, LeagueID = ev.LeagueID, EventNumber = ev.EventNumber, Results = tempResults, cssId = "collapse" + ev.Id });
            }

            LeagueEventVMs = LeagueEventVMs.OrderBy(d => d.Date).ToList();
        }
    }
}
