using Microsoft.AspNetCore.Html;
using PioneerLigan.HelperClasses;
using PioneerLigan.Models;

namespace PioneerLigan.ViewModels
{
    public class _PlayerVM
    {
        public string Name { get; set; } = String.Empty;
        public int Events { get; set; } = 0;
        public int CurrentLeaguePoints { get; set; } = 0;
        public int DiscountedPoints { get; set; } = 0;
        public List<ResultObject> PlayerResults { get; set; } = new List<ResultObject>();
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Ties { get; set; } = 0;
        public int FourZero { get; set; } = 0;
        public int ThreeZeroOne { get; set; } = 0;
        public int ThreeOne { get; set; } = 0;
        public int TuTu { get; set; } = 0;
        public int LifeTimePoints { get; set; } = 0;
        public double AvgPoints { get; set; } = 0;
        public HtmlString StatBox { get; set; }

        public _PlayerVM(Models.Player player, _LeagueVM league)
        {
            int id = 0;
            Name = player.PlayerName;
            Events = player.Events;
            CurrentLeaguePoints = 0;
            PlayerResults = new List<ResultObject>();
            LifeTimePoints = player.Points;
            Wins = player.Wins;
            Ties = player.Ties;
            Losses = player.Losses;
            AvgPoints = player.Points / (double)player.Events;

            foreach (var ev in league.Events)
            {
                var er = league.Results.Where(i => i.PlayerId == player.Id && i.EventId == ev.Id);

                if (er.Any())
                {
                    PlayerResults.Add(new ResultObject(id, er.First().Points, true, true));
                    CurrentLeaguePoints += er.First().Points;
                    AddTieBreakers(er.First().Points);
                }
                else
                {
                    PlayerResults.Add(new ResultObject(id, 0, true, false));
                }
                id++;
            }
            CalculatePoints();
            StatBox = BuildStatBox();
        }


        private HtmlString BuildStatBox()
        {
            float playedMatches = Wins + Ties + Losses;
            float winPercentage = (Wins / playedMatches) * 100;
            HtmlString returnString = new HtmlString(string.Format(@"<button href='abc' class='player-stats' data-bs-toggle='tooltip' data-bs-html='true'
                            data-bs-placement='left' data-bs-title='Lifetime stats:<br>Points: {0}<br>Wins: {1}<br>Ties: {2}<br>Losses: {3}<br>Average points: {4}<br>
                            Win %: {5}%'>{6}</button>",
                           LifeTimePoints, Wins, Ties, Losses, AvgPoints.ToString("0.00"), winPercentage.ToString("0.00"), Name));

            return returnString;
        }

        private void AddTieBreakers(int points)
        {
            switch (points)
            {
                case 6:
                    TuTu++;
                    break;
                case 9:
                    ThreeOne++;
                    break;
                case 10:
                    ThreeZeroOne++;
                    break;
                case 12:
                    FourZero++;
                    break;
                default:
                    break;
            }
        }

        private void CalculatePoints()
        {
            List<ResultObject> returnResults = PlayerResults;
            returnResults = returnResults.OrderByDescending(p => p.Result).ToList();
            returnResults = returnResults.Take(7).ToList();
            foreach (var r in PlayerResults)
            {
                if (returnResults.Contains(r))
                {
                    r.CountThis = true;
                }
                else
                {
                    r.CountThis = false;
                }

                if (r.CountThis)
                {
                    DiscountedPoints += r.Result;
                }
            }
        }
    }
}
