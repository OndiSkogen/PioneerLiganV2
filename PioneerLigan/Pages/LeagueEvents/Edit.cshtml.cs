using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PioneerLigan.Data;
using PioneerLigan.Models;

namespace PioneerLigan.Pages.LeagueEvents
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public EditModel(PioneerLigan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LeagueEvent LeagueEvent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.LeagueEvents == null)
            {
                return NotFound();
            }

            var leagueevent =  await _context.LeagueEvents.FirstOrDefaultAsync(m => m.Id == id);
            if (leagueevent == null)
            {
                return NotFound();
            }
            LeagueEvent = leagueevent;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(LeagueEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueEventExists(LeagueEvent.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LeagueEventExists(int id)
        {
          return (_context.LeagueEvents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
