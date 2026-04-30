using DVD_Orama_Services_rest.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DVD_Orama_Services_rest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieCollection> MovieCollections { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<User> Users => Set<User>();
        //public DbSet<MovieCollectionItem> MovieCollection => Set<MovieCollectionItem>();

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<MovieCollectionItem>()
        //        .HasOne(m => m.User)
        //        .WithMany(u => u.Movies)
        //        .HasForeignKey(m => m.UserId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<MovieCollectionItem>()
        //        .Property(m => m.Barcode)
        //        .IsRequired()
        //        .HasMaxLength(50);
        //}
    }
}
