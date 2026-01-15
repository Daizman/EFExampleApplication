using EFExampleApplication.Abstractions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class MovieRepository : IMovieRepository
{
    private readonly IReadOnlySet<Genre> _genres = new HashSet<Genre>
    {
        new() { Id = 1, Name = "Action" },
        new() { Id = 2, Name = "Comedy" },
        new() { Id = 3, Name = "Drama" },
        new() { Id = 4, Name = "Horror" },
        new() { Id = 5, Name = "Sci-Fi" },
    };
    private readonly List<GenreInMovie> _genresInMovies = new();
    private readonly List<Movie> _movies = new();

    public IReadOnlyList<Movie> GetMovies() => _movies;

    public Movie? GetMovie(int movieId)
    {
        var movie = _movies.FirstOrDefault(g => g.Id == movieId);

        if (movie is null)
        {
            return null;
        }

        var genres = _genresInMovies
            .Where(g => g.MovieId == movieId)
            .ToList();
        movie.Genres = genres;

        return movie;
    }

    public int AddMovie(Movie movie)
    {
        movie.Id = _movies.Count + 1;
        _movies.Add(movie);

        return movie.Id;
    }

    public bool UpdateGenresForMovie(int movieId, int[] newGenreIds)
    {
        var movie = _movies.FirstOrDefault(g => g.Id == movieId);

        if (movie is null)
        {
            return false;
        }

        var allGenresExists = newGenreIds.All(CheckIfGenreExists);

        if (!allGenresExists)
        {
            return false;
        }

        _genresInMovies.RemoveAll(g => g.MovieId == movieId);
        foreach (var genreId in newGenreIds)
        {
            var genreInMovie = new GenreInMovie
            {
                MovieId = movie.Id,
                Movie = movie,
                GenreId = genreId,
                Genre = _genres.First(g => g.Id == genreId),
            };
            if (!_genresInMovies.Contains(genreInMovie))
            {
                _genresInMovies.Add(genreInMovie);
            }
        }

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
        _genresInMovies.RemoveAll(g => g.MovieId == movieId);

        return true;
    }

    private bool CheckIfGenreExists(int genreId)
    {
        var genre = _genres.FirstOrDefault(g => g.Id == genreId);
        if (genre is null)
        {
            return false;
        }

        return true;
    }
}
