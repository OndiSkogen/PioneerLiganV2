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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.MetaGame> MetaGame { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.MetaGames != null)
            {
                MetaGame = await _context.MetaGames.ToListAsync();
            }
        }
    }
}
