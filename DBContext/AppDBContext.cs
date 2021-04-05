using Accepted.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accepted.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchOdd> MatchOdds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>().HasIndex(b => b.TeamA);
            modelBuilder.Entity<Match>().HasIndex(b => b.TeamB);
            modelBuilder.Entity<Match>().HasIndex(b => new { b.TeamA, b.TeamB });
            modelBuilder.Entity<Match>().HasIndex(b => new { b.TeamA, b.TeamB, b.MatchDate }).IsUnique();
            modelBuilder.Entity<Match>().HasIndex(b => b.MatchDate);
        }
    }
}
