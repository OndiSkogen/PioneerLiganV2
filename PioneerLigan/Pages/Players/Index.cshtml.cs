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

namespace PioneerLigan.Pages.Players
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public IndexModel(PioneerLigan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Player> Player { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Player != null)
            {
                Player = await _context.Player.ToListAsync();
            }
        }
    }
}
