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
    public class DetailsModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public DetailsModel(PioneerLigan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public LeagueEvent LeagueEvent { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.LeagueEvents == null)
            {
                return NotFound();
            }

            var leagueevent = await _context.LeagueEvents.FirstOrDefaultAsync(m => m.Id == id);
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
    }
}
