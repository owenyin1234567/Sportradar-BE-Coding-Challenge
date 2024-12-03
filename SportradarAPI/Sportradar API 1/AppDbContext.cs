using Microsoft.EntityFrameworkCore;
using Sportradar_API.Model;
using Sportradar_API_1.Model;

namespace Sportradar_API
{
    public class AppDbContext : DbContext
    {
        public DbSet<Country>? Countries { get; set; }
        public DbSet<Team>? Teams { get; set; }
        public DbSet<Sport>? Sports { get; set; }
        public DbSet<Championship>? Championships { get; set; }
        public DbSet<Season>? Seasons { get; set; }
        public DbSet<Result>? Results { get; set; }
        public DbSet<Game>? Games { get; set; }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
            .HasKey(c => c.Abbr);

            modelBuilder.Entity<Country>()
            .Property(c => c.Name)
            .HasMaxLength(16);

            modelBuilder.Entity<Country>()
            .Property(c => c.Name)
            .HasMaxLength(256);

            modelBuilder.Entity<Team>()
            .HasKey(t => t.Slug);

            modelBuilder.Entity<Team>()
            .HasOne<Country>()
            .WithMany()
            .HasForeignKey(t => t.Country);

            modelBuilder.Entity<Sport>()
    .       HasKey(s => s.ID);

            modelBuilder.Entity<Sport>()
            .Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(256); 

            modelBuilder.Entity<Championship>()
            .HasKey(c => c.ID);

            modelBuilder.Entity<Championship>()
            .Property(c => c.Name)
            .HasMaxLength(256);

            modelBuilder.Entity<Championship>()
            .HasOne<Sport>()
            .WithMany()
            .HasForeignKey(c => c.Sport);

            modelBuilder.Entity<Season>()
            .HasKey(s => s.ID);

            modelBuilder.Entity<Season>()
            .HasOne<Championship>()
            .WithMany()
            .HasForeignKey(s => s.Championship);
            
            modelBuilder.Entity<Result>()
            .HasKey(r => r.ID);

            modelBuilder.Entity<Result>()
            .HasOne<Team>()
            .WithMany()
            .HasForeignKey(r => r.Winner);

            modelBuilder.Entity<Game>()
    .       HasKey(g => g.ID);

            modelBuilder.Entity<Game>()
            .HasOne<Team>()
            .WithMany()
            .HasForeignKey(g => g.HomeTeam_FK);

            modelBuilder.Entity<Game>()
            .HasOne<Team>()
            .WithMany()
            .HasForeignKey(g => g.AwayTeam_FK) 
            .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Game>()
           .HasOne<Season>()
           .WithMany()
           .HasForeignKey(g => g.Season_FK); 

            modelBuilder.Entity<Game>()
            .HasOne<Result>()
            .WithMany()
            .HasForeignKey(g => g.Result_FK); 
        }*/
    }

    }



