using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PioneerLigan.Data;
using PioneerLigan.Models;

namespace PioneerLigan.Pages.MetaGame
{
    public class DeleteModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public DeleteModel(PioneerLigan.Data.ApplicationDbContext context)
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

            var metagame = await _context.MetaGames.FirstOrDefaultAsync(m => m.Id == id);

            if (metagame == null)
            {
                return NotFound();
            }
            else 
            {
                MetaGame = metagame;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.MetaGames == null)
            {
                return NotFound();
            }
            var metagame = await _context.MetaGames.FindAsync(id);

            if (metagame != null)
            {
                MetaGame = metagame;
                _context.MetaGames.Remove(MetaGame);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
