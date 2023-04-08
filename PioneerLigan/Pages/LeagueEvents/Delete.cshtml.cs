using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PioneerLigan.Data;
using PioneerLigan.Models;

namespace PioneerLigan.Pages.LeagueEvents
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public DeleteModel(PioneerLigan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public LeagueEvent LeagueEvent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.LeagueEvent == null)
            {
                return NotFound();
            }

            var leagueevent = await _context.LeagueEvent.FirstOrDefaultAsync(m => m.Id == id);

            if (leagueevent == null)
            {
                return NotFound();
            }
            else 
            {
                LeagueEvent = leagueevent;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.LeagueEvent == null)
            {
                return NotFound();
            }
            var leagueevent = await _context.LeagueEvent.FindAsync(id);

            if (leagueevent != null)
            {
                LeagueEvent = leagueevent;
                _context.LeagueEvent.Remove(LeagueEvent);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
