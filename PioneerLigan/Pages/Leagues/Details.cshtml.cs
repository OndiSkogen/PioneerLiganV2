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

namespace PioneerLigan.Pages.Leagues
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public DetailsModel(PioneerLigan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public League League { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Leagues == null)
            {
                return NotFound();
            }

            var league = await _context.Leagues.FirstOrDefaultAsync(m => m.Id == id);
            if (league == null)
            {
                return NotFound();
            }
            else 
            {
                League = league;
            }
            return Page();
        }
    }
}
