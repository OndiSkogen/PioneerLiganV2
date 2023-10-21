using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PioneerLigan.Data;
using PioneerLigan.ViewModels;

namespace PioneerLigan.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<_LeagueVM> LeagueVMs { get; set; } = new List<_LeagueVM>();
        public List<_PlayerVM> TopPlayers { get; set; } = new List<_PlayerVM>();
        public _MetaGameVM? MetaGame { get; set; }

        public void OnGet()
        {
            if (_context.League != null)
            {
                var leagues = _context.League.OrderByDescending(i => i.Id).ToList();
                var players = from p in _context.Player select p;
                var topPlayers = players.OrderByDescending(p => p.Points).Take(10).ToList();
                var eventResults = from e in _context.EventResult select e;
                var events = from e in _context.LeagueEvent select e;

                foreach (var player in topPlayers)
                {
                    TopPlayers.Add(new _PlayerVM(player, leagues));
                }

                foreach (var league in leagues)
                {

                    var tempLeague = new _LeagueVM(events.Where(e => e.LeagueID == league.Id).ToList(), players.ToList(), eventResults.ToList(), league);

                    foreach (var player in tempLeague.Players)
                    {
                        _PlayerVM tempPlayer = new _PlayerVM(player, tempLeague);
                        tempLeague.PlayersVMs.Add(tempPlayer);
                    }

                    tempLeague.PlayersVMs = tempLeague.PlayersVMs.Where(p => p.CurrentLeaguePoints > 0).OrderByDescending(p => p.DiscountedPoints).ThenByDescending(p => p.FourZero).ThenByDescending(p => p.ThreeZeroOne)
                            .ThenByDescending(p => p.ThreeOne).ThenByDescending(p => p.TuTu).ThenByDescending(p => p.Events).ToList();

                    LeagueVMs.Add(tempLeague);
                }

                if (events.Any())
                {
                    //var daEvent = events.OrderByDescending(e => e.Date).First();
                    //MetaGame = new _MetaGameVM(daEvent.MetaGame, daEvent.Date);
                    MetaGame = null;
                }
                else
                {
                    MetaGame = null;
                }
            }
        }
    }
}