using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PioneerLigan.Data;
using PioneerLigan.Models;
using PioneerLigan.Constants;

namespace PioneerLigan.Pages.MetaGame
{
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

        // Retrieve LeagueEventId from query string
        public List<Deck> ExistingDecks { get; set; }
        public int MetaGameId { get; set; }
        public Dictionary<string, int> DeckCounts { get; set; }

        public IActionResult OnGet(int leagueEventId, int metaGameId)
        {
            LoadData();

            if (leagueEventId != 0)
            {
                MetaGame = new Models.MetaGame();
                MetaGame.LeagueEvent = _context.LeagueEvents.FirstOrDefault(e => e.Id == leagueEventId);
                MetaGameId = MetaGame.Id;
            }
            else if (metaGameId != 0)
            {
                MetaGame = _context.MetaGames.FirstOrDefault(m => m.Id == metaGameId);
                MetaGameId = metaGameId;
                if (MetaGame == null)
                {
                    return RedirectToPage("./Index");
                }

                foreach (var deck in MetaGame.Decks)
                {
                    AddDeckToMetaGame(deck.Name);
                }
            }
            else
            {
                return RedirectToPage("./Index");
            }

            Deck = new Deck();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action, int id)
        {
            if (action == "AddDeck")
            {
                if (MetaGame == null || Deck == null)
                {
                    LoadData();
                    return Page();
                }

                if (id != 0)
                {
                    MetaGame = _context.MetaGames.FirstOrDefault(m => m.Id == id);
                }

                if (MetaGame.Id == 0)
                {
                    _context.MetaGames.Add(MetaGame);
                    await _context.SaveChangesAsync();
                }

                if (Deck.Id != 0)
                {
                    // User selected an existing deck
                    LoadData();
                    var selectedDeck = ExistingDecks.FirstOrDefault(d => d.Id == Deck.Id);
                    if (selectedDeck != null)
                    {
                        var newDeck = new Deck
                        {
                            Name = selectedDeck.Name,
                            SuperArchType = selectedDeck.SuperArchType,
                            ColorAffiliation = selectedDeck.ColorAffiliation,
                            MetaGame = MetaGame
                        };

                        _context.Decks.Add(newDeck);

                        _context.Update(MetaGame);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    var newDeck = new Deck
                    {
                        Name = Deck.Name,
                        SuperArchType = Deck.SuperArchType,
                        ColorAffiliation = Deck.ColorAffiliation,
                        MetaGame = MetaGame
                    };

                    _context.Decks.Add(newDeck);

                    _context.Update(MetaGame);
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("../MetaGame/Create", new { metaGameId = MetaGame.Id });
            }
            else if (action == "Done")
            {                
                return RedirectToPage("/Index");
            }

            LoadData();
            return Page();
        }

        private void LoadData()
        {
            var allDecks = _context.Decks.ToList();

            ExistingDecks = allDecks.GroupBy(deck => deck.Name)
                                    .Select(group => group.First())
                                    .ToList();

            DeckCounts = new Dictionary<string, int>();
        }

        private void AddDeckToMetaGame(string deckName)
        {
            if (DeckCounts.ContainsKey(deckName))
            {
                DeckCounts[deckName]++;
            }
            else
            {
                DeckCounts[deckName] = 1;
            }
        }
    }
}
