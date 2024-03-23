using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PioneerLigan.Data;
using PioneerLigan.Models;

namespace PioneerLigan.Pages.Players
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public CreateModel(PioneerLigan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Player Player { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Players == null || Player == null)
            {
                return Page();
            }

            _context.Players.Add(Player);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
