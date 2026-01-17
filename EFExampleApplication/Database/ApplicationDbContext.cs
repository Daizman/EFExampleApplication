using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<GenreInMovie> GenreInMovies { get; set; }
    public DbSet<Review> Reviews { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GenreInMovie>()
            .HasKey(gm => new { gm.MovieId, gm.GenreId });

        AddDefaultGenres(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void AddDefaultGenres(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new() { Id = 1, Name = "Action" },
            new() { Id = 2, Name = "Comedy" },
            new() { Id = 3, Name = "Drama" },
            new() { Id = 4, Name = "Horror" },
            new() { Id = 5, Name = "Sci-Fi" }
        );
    }
}