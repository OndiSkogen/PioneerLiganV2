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

namespace PioneerLigan.Pages.Leagues
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
        public League League { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Leagues == null)
            {
                return NotFound();
            }

            var league =  await _context.Leagues.FirstOrDefaultAsync(m => m.Id == id);
            if (league == null)
            {
                return NotFound();
            }
            League = league;
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

            _context.Attach(League).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueExists(League.Id))
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

        private bool LeagueExists(int id)
        {
          return (_context.Leagues?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
