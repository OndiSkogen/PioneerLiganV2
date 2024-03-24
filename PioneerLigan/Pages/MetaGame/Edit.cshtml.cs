using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PioneerLigan.Data;
using PioneerLigan.Models;

namespace PioneerLigan.Pages.MetaGame
{
    public class EditModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public EditModel(PioneerLigan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.MetaGame MetaGame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.MetaGames == null)
            {
                return NotFound();
            }

            var metagame =  await _context.MetaGames.FirstOrDefaultAsync(m => m.Id == id);
            if (metagame == null)
            {
                return NotFound();
            }
            MetaGame = metagame;
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

            _context.Attach(MetaGame).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetaGameExists(MetaGame.Id))
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

        private bool MetaGameExists(int id)
        {
          return (_context.MetaGames?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
