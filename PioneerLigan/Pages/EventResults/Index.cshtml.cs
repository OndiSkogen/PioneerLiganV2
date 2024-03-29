﻿using System;
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
    public class IndexModel : PageModel
    {
        private readonly PioneerLigan.Data.ApplicationDbContext _context;

        public IndexModel(PioneerLigan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EventResult> EventResult { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.EventResults != null)
            {
                EventResult = await _context.EventResults.ToListAsync();
            }
        }
    }
}
