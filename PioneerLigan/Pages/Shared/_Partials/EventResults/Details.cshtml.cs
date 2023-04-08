using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PioneerLigan.Data;
using PioneerLigan.Models;

namespace PioneerLigan.Pages._Partials.EventResults
{
    public class DetailsModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public DetailsModel(PioneerLigan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public EventResult EventResult { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.EventResult == null)
            {
                return NotFound();
            }

            var eventresult = await _context.EventResult.FirstOrDefaultAsync(m => m.Id == id);
            if (eventresult == null)
            {
                return NotFound();
            }
            else 
            {
                EventResult = eventresult;
            }
            return Page();
        }
    }
}
