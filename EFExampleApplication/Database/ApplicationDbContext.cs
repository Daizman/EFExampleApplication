using EFExampleApplication.Configurations.Database;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EFExampleApplication.Database;

public class ApplicationDbContext : DbContext
{
    private readonly ApplicationDbContextSettings _dbContextSettings;

    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<GenreInMovie> GenreInMovies { get; set; }
    public DbSet<Review> Reviews { get; set; }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<ApplicationDbContextSettings> dbContextSettings
    ) : base(options)
    {
        _dbContextSettings = dbContextSettings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_dbContextSettings.ConnectionString);
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Debug);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }
}
