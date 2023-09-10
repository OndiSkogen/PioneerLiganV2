using Microsoft.AspNetCore.Identity;
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
        public DbSet<EventResult> EventResult { get; set; } = default!;
        public DbSet<LeagueEvent> LeagueEvent { get; set; } = default!;
        public DbSet<League> League { get; set; } = default!;
        public DbSet<Player> Player { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventResult>()
                .Property(e => e.OMW)
                .HasColumnType("decimal(5, 2)");

            modelBuilder.Entity<EventResult>()
                .Property(e => e.OGW)
                .HasColumnType("decimal(5, 2)");

            modelBuilder.Entity<EventResult>()
                .Property(e => e.GW)
                .HasColumnType("decimal(5, 2)");

            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasNoKey();

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasNoKey();

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasNoKey();
        }
    }
}