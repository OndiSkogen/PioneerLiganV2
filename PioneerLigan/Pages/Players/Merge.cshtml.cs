using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PioneerLigan.Data;
using PioneerLigan.Models;

namespace PioneerLigan.Pages.Players
{
    public class MergeModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        
        public MergeModel(ApplicationDbContext context)
        {
            _context = context;
            Players = _context.Players.OrderByDescending(p => p.PlayerName).ToList();
        }

        [BindProperty]
        public Player Player1 { get; set; } = new Player();

        [BindProperty]
        public Player Player2 { get; set; } = new Player();


        public List<Player> Players;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var success = await MergePlayers(Player1, Player2);

            if (success)
            {
                return RedirectToPage("/Admin/Index");
            }
            else
            {
                return NotFound();
            }
        }

        private async Task<bool> MergePlayers(Player player1, Player player2)
        {
            try
            {
                var existingPlayer1 = await _context.Players.FindAsync(player1.Id);
                var existingPlayer2 = await _context.Players.FindAsync(player2.Id);

                if (existingPlayer1 == null || existingPlayer2 == null)
                {
                    return false;
                }

                existingPlayer1.Events += existingPlayer2.Events;
                existingPlayer1.Points += existingPlayer2.Points;
                existingPlayer1.Wins += existingPlayer2.Wins;
                existingPlayer1.Losses += existingPlayer2.Losses;
                existingPlayer1.Ties += existingPlayer2.Ties;

                _context.Players.Update(existingPlayer1);
                _context.Players.Remove(existingPlayer2);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
