using EFExampleApplication.Abstractions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Database;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Movie> Movies { get; set; }
    public virtual DbSet<Genre> Genres { get; set; }
    public virtual DbSet<GenreInMovie> GenreInMovies { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
