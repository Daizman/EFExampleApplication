using EFExampleApplication.Abstractions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Services;

public class MovieRepository(IApplicationDbContext applicationDbContext) : IMovieRepository
{
    private readonly DbSet<Genre> _genres = applicationDbContext.Genres;
    private readonly DbSet<GenreInMovie> _genresInMovies = applicationDbContext.GenreInMovies;
    private readonly DbSet<Movie> _movies = applicationDbContext.Movies;

    public IReadOnlyList<Movie> GetMovies() => _movies.AsNoTracking().ToList();

    public Movie? GetMovie(int movieId)
    {
        var movie = _movies.AsNoTracking()
            .Include(m => m.Genres)
            .ThenInclude(gInM => gInM.Genre)
            .FirstOrDefault(g => g.Id == movieId);

        if (movie is null)
        {
            return null;
        }

        return movie;
    }

    public int AddMovie(Movie movie)
    {
        _movies.Add(movie);

        applicationDbContext.SaveChanges();

        return movie.Id;
    }

    public bool UpdateGenresForMovie(int movieId, int[] newGenreIds)
    {
        var movie = _movies.FirstOrDefault(g => g.Id == movieId);

        if (movie is null)
        {
            return false;
        }

        var existingGenres = _genres.Where(g => newGenreIds.Contains(g.Id)).ToList();

        if (existingGenres.Count != newGenreIds.Length)
        {
            return false;
        }

        var oldMovieGenres = _genresInMovies.Where(g => g.MovieId == movieId).ToList();
        _genresInMovies.RemoveRange(oldMovieGenres);
        foreach (var genreId in newGenreIds)
        {
            var genreInMovie = new GenreInMovie
            {
                MovieId = movie.Id,
                Movie = movie,
                GenreId = genreId,
                Genre = _genres.First(g => g.Id == genreId),
            };
           _genresInMovies.Add(genreInMovie);
        }

        applicationDbContext.SaveChanges();

        return true;
    }

    public bool UpdateMovie(
        int movieId, 
        string? title, 
        string? description, 
        int? durationInMinutes
    )
    {
        var oldVersion = _movies.FirstOrDefault(g => g.Id == movieId);

        if (oldVersion is null)
        {
            return false;
        }

        oldVersion.Title = title ?? oldVersion.Title;
        oldVersion.Description = description ?? oldVersion.Description;
        oldVersion.DurationInMinutes = durationInMinutes ?? oldVersion.DurationInMinutes;
        
        applicationDbContext.SaveChanges();

        return true;
    }

    public bool DeleteMovie(int movieId)
    {
        var movie = _movies.FirstOrDefault(g => g.Id == movieId);

        if (movie is null)
        {
            return false;
        }

        _movies.Remove(movie);

        applicationDbContext.SaveChanges();

        return true;
    }
}
