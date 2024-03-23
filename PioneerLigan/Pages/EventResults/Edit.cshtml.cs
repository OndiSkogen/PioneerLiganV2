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

namespace PioneerLigan.Pages._Partials.EventResults
{
    public class EditModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public EditModel(PioneerLigan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EventResult EventResult { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.EventResults == null)
            {
                return NotFound();
            }

            var eventresult =  await _context.EventResults.FirstOrDefaultAsync(m => m.Id == id);
            if (eventresult == null)
            {
                return NotFound();
            }
            EventResult = eventresult;
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

            _context.Attach(EventResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventResultExists(EventResult.Id))
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

        private bool EventResultExists(int id)
        {
          return (_context.EventResults?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
