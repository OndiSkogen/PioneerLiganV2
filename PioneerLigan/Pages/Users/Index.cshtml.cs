using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PioneerLigan.Data;
using PioneerLigan.Models;
using PioneerLigan.ViewModels;

namespace PioneerLigan.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<User> Users { get; set; } = default!;
        public List<Player> Players { get; set; } = new List<Player>();
        public Player ActiveUser { get; set; } = new Player();
        public _UserVM ActiveUserVM { get; set; } = default!;
        //public async Task OnGetAsync()
        //{
        //    //if (_context.User != null)
        //    //{
        //    //    Users = await _context.User.ToListAsync();
        //    //    Players = await _context.Player.ToListAsync();

        //    //    if (Users.FirstOrDefault() != null && _context.Player != null)
        //    //    {
        //    //        ActiveUser = await _context.Player.FirstOrDefaultAsync(u => u.Id == Users.FirstOrDefault().PlayerId);
        //    //    }
        //    //}

        //    //ActiveUserVM = new _UserVM(ActiveUser);
        //}

        public IActionResult OnGet()
        {
            var user = HttpContext.User; // This gets the current user's claims
            var userId = _userManager.GetUserId(user); // Get the user's ID

            return Page();
        }
    }
}
