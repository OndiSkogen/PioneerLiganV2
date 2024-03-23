using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PioneerLigan.Data;
using PioneerLigan.Models;

namespace PioneerLigan.Pages._Partials.EventResults
{
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
        public EventResult EventResult { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.EventResults == null || EventResult == null)
            {
                return Page();
            }

            _context.EventResults.Add(EventResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
