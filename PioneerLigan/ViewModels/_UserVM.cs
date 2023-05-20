using PioneerLigan.Models;

namespace PioneerLigan.ViewModels
{
    public class _UserVM
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int FourOhs { get; set; }
        public int ThreeOnes { get; set; }
        public double WinPercentage { get; set; }
        public int LeagueWins { get; set; }
        public int GroupStageWins { get; set; }
        public List<_EventResultVM> LatestScores { get; set; } = new List<_EventResultVM>();
    }
}
