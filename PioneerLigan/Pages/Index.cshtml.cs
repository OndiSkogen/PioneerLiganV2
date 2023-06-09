﻿using Microsoft.AspNetCore.Authorization;
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

        public List<ViewModels._LeagueVM> LeagueVMs { get; set; } = new List<ViewModels._LeagueVM>();
        public List<Models.League> Leagues { get; set; } = new List<Models.League>();

        public void OnGet()
        {
            if (_context.League != null)
            {
                Leagues = _context.League.OrderByDescending(i => i.Id).ToList();

                foreach (var league in Leagues)
                {
                    var events = from e in _context.LeagueEvent where e.LeagueID == league.Id select e;
                    var eventResults = from e in _context.EventResult select e;
                    var players = from p in _context.Player select p;

                    var tempLeague = new _LeagueVM(events.ToList(), players.ToList(), eventResults.ToList(), league);

                    foreach (var player in tempLeague.Players)
                    {
                        _PlayerVM tempPlayer = new _PlayerVM(player, tempLeague);
                        tempLeague.PlayersVMs.Add(tempPlayer);
                    }

                    tempLeague.PlayersVMs = tempLeague.PlayersVMs.Where(p => p.CurrentLeaguePoints > 0).OrderByDescending(p => p.DiscountedPoints).ThenByDescending(p => p.FourZero).ThenByDescending(p => p.ThreeZeroOne)
                            .ThenByDescending(p => p.ThreeOne).ThenByDescending(p => p.TuTu).ThenByDescending(p => p.Events).ToList();

                    LeagueVMs.Add(tempLeague);
                }
            }
        }
    }
}