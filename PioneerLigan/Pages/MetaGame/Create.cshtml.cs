using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PioneerLigan.Data;
using PioneerLigan.Models;
using PioneerLigan.Constants;
using static PioneerLigan.Pages.MetaGame.CreateModel;

namespace PioneerLigan.Pages.MetaGame
{
    [ValidateAntiForgeryToken]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.MetaGame MetaGame { get; set; }

        [BindProperty]
        public Deck Deck { get; set; }


        public List<Deck> ExistingDecks { get; set; }
        public int LeagueEventId { get; set; }

        public IActionResult OnGet(int leagueEventId)
        {
            LoadData();

            if (leagueEventId != 0)
            {
                LeagueEventId = leagueEventId;
                var leagueEvent = _context.LeagueEvents.FirstOrDefault(e => e.Id == leagueEventId);
                MetaGame = _context.MetaGames.FirstOrDefault(m => m.LeagueEvent == leagueEvent);
            }
            else
            {
                return RedirectToPage("./Index");
            }

            Deck = new Deck();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromBody] MetaGameData metaGameData)
        {
            if (metaGameData != null)
            {
                var leagueEvent = _context.LeagueEvents.FirstOrDefault(e => e.Id == metaGameData.LeagueEventId);
                MetaGame = _context.MetaGames.FirstOrDefault(m => m.LeagueEvent == leagueEvent);
                
                if (MetaGame == null && leagueEvent != null)
                {
                    MetaGame = new Models.MetaGame();
                    MetaGame.LeagueEvent = leagueEvent;
                    _context.MetaGames.Add(MetaGame);
                    await _context.SaveChangesAsync();
                }

                if (MetaGame != null)
                {
                    foreach (var newDeck in metaGameData.NewDecks)
                    {
                        MetaGame.Decks.Add(newDeck);
                        _context.Decks.Add(newDeck);
                    }

                    foreach (var existingDeck in metaGameData.ExistingDecks)
                    {
                        LoadData();
                        var selectedDeck = ExistingDecks.FirstOrDefault(d => d.Id == existingDeck.Id);
                        if (selectedDeck != null)
                        {
                            var newDeck = new Deck
                            {
                                Name = selectedDeck.Name,
                                SuperArchType = selectedDeck.SuperArchType,
                                ColorAffiliation = selectedDeck.ColorAffiliation,
                                MetaGame = MetaGame
                            };

                            MetaGame.Decks.Add(newDeck);
                            _context.Decks.Add(newDeck);

                            await _context.SaveChangesAsync();
                        }
                    }

                    _context.Update(MetaGame);
                    await _context.SaveChangesAsync();

                    return RedirectToPage("./Index");
                }

                return BadRequest("Can't find league event.");
            }

            return BadRequest("Invalid data received");            
        }

        private void LoadData()
        {
            var allDecks = _context.Decks.ToList();

            ExistingDecks = allDecks.GroupBy(deck => deck.Name)
                                    .Select(group => group.First())
                                    .OrderByDescending(d => d.Name)
                                    .ToList();
        }

        public class MetaGameData
        {
            public List<Deck> NewDecks { get; set; } = new List<Deck>();
            public List<ExistingDeck> ExistingDecks { get; set; } = new List<ExistingDeck>();
            public int LeagueEventId { get; set; }
        }

        public class ExistingDeck
        {
            public int Id { get; set; }
        }
    }
}
