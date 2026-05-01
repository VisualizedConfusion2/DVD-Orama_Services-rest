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
        public DbSet<UserMovieCollectionMap> UserMovieCollectionMap { get; set; }
        public DbSet<MovieCollectionsMoviesMap> MovieCollectionsMoviesMap { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite PKs
            modelBuilder.Entity<MovieCollectionsMoviesMap>()
                .HasKey(m => new { m.MovieId, m.MovieCollectionId });

            modelBuilder.Entity<UserMovieCollectionMap>()
                .HasKey(u => new { u.UserId, u.MovieCollectionId });

            // Ignore list properties on Movie that don't exist as columns
            modelBuilder.Entity<Movie>()
                .Ignore(m => m.Actors)
                .Ignore(m => m.Genres)
                .Ignore(m => m.EANs)
                .Ignore(m => m.StreamingServices);

            // Ignore Movies navigation on MovieCollection to prevent EF adding MovieCollectionId FK
            modelBuilder.Entity<MovieCollection>()
                .Ignore(c => c.Movies)
                .Ignore(c => c.AdminIds)
                .Ignore(c => c.RegUserIds)
                .Ignore(c => c.ViewerIds)
                .Ignore(c => c.OwnerId);

            // Map correct PK column name
            modelBuilder.Entity<Movie>()
                .Property(m => m.Id)
                .HasColumnName("MovieId");

            modelBuilder.Entity<MovieCollection>()
                .Property(c => c.Id)
                .HasColumnName("MovieCollectionId");
        }
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
