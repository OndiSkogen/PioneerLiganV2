using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public Dictionary<string, int> MetaGame { get; set; }
        public DateTime LatestEvent { get; set; }

        public void OnGet()
        {
            if (_context.Leagues != null)
            {
                var leagues = _context.Leagues.OrderByDescending(i => i.Id).ToList();
                var players = from p in _context.Players select p;
                var topPlayers = players.OrderByDescending(p => p.Points).Take(10).ToList();
                var eventResults = from e in _context.EventResults select e;
                var events = from e in _context.LeagueEvents select e;
                var metagames = _context.MetaGames
                                .Include(m => m.Decks)
                                .ToList();

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
                    var daEvent = events.OrderByDescending(e => e.Date).First();
                    LatestEvent = daEvent.Date;
                    var metaGame = metagames.Where(m => m.LeagueEvent.Id == daEvent.Id).FirstOrDefault();
                    if (metaGame == null)
                    {
                        MetaGame = new Dictionary<string, int>();
                    }
                    else
                    {
                        MetaGame = new Dictionary<string, int>();
                        foreach (var deck in metaGame.Decks)
                        {
                            AddDeckToMetaGame(deck.Name);
                        }
                    }
                }
                else
                {
                    MetaGame = new Dictionary<string, int>();
                }
            }
        }

        private void AddDeckToMetaGame(string deckName)
        {
            if (MetaGame.ContainsKey(deckName))
            {
                MetaGame[deckName]++;
            }
            else
            {
                MetaGame[deckName] = 1;
            }
        }
    }
}