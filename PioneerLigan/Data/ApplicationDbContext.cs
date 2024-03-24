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
        public DbSet<EventResult> EventResults { get; set; } = default!;
        public DbSet<LeagueEvent> LeagueEvents { get; set; } = default!;
        public DbSet<League> Leagues { get; set; } = default!;
        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Deck> Decks { get; set; } = default!;
        public DbSet<MetaGame> MetaGames { get; set; } = default!;

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