using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Abstractions;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; }
    public DbSet<Movie> Movies { get; }
    public DbSet<Genre> Genres { get; }
    public DbSet<GenreInMovie> GenreInMovies { get; }
    public DbSet<Review> Reviews { get; }

    public int SaveChanges();
}
