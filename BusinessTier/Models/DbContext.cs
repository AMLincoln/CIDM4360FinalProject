using System;
using Microsoft.EntityFrameworkCore;

namespace FinalProject
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users {get; set;} = null!;
        public DbSet<ResidentPackage> ResidentPackages {get; set;} = null!;
        public DbSet<UnknownPackage> UnknownPackages {get; set;} = null!;
        public DbSet<Resident> Residents {get; set;} = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>().ToTable("Packages");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = database.db");
        }
        
    }
}