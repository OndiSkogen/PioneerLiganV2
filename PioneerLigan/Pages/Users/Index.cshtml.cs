using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PioneerLigan.Data;
using PioneerLigan.Models;
using PioneerLigan.ViewModels;

namespace PioneerLigan.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<User> Users { get; set; } = default!;
        public List<Player> Players { get; set; }
        public Player ActiveUser { get; set; }
        public _UserVM ActiveUserVM { get; set; }
        public async Task OnGetAsync()
        {
            if (_context.User != null)
            {
                Users = await _context.User.ToListAsync();
                Players = await _context.Player.ToListAsync();

                if (Users.FirstOrDefault() != null)
                {
                    ActiveUser = await _context.Player.FirstOrDefaultAsync(u => u.Id == Users.FirstOrDefault().PlayerId);
                }
            }

            ActiveUserVM = new _UserVM(ActiveUser);
        }
    }
}
