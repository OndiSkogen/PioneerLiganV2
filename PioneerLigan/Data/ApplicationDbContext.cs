using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PioneerLigan.Models;

namespace PioneerLigan.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PioneerLigan.Models.EventResult> EventResult { get; set; } = default!;
        public DbSet<PioneerLigan.Models.LeagueEvent> LeagueEvent { get; set; } = default!;
        public DbSet<PioneerLigan.Models.League> League { get; set; } = default!;
        public DbSet<PioneerLigan.Models.Player> Player { get; set; } = default!;
    }
}