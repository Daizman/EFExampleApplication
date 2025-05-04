using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class MovieRepository(
    IMapper mapper
) : IMovieRepository
{
    private readonly IReadOnlySet<Genre> _genres = new HashSet<Genre>
    {
        new() { Id = 1, Name = "Action" },
        new() { Id = 2, Name = "Comedy" },
        new() { Id = 3, Name = "Drama" },
        new() { Id = 4, Name = "Horror" },
        new() { Id = 5, Name = "Sci-Fi" }
    };
    private readonly List<GenreInMovie> _genresInMovies = new();
    private readonly List<Movie> _movies = new();

    public ListOfMovies GetMovies() => mapper.Map<ListOfMovies>(_movies);

    public MovieVm GetMovie(int id)
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);

        var genres = _genresInMovies
            .Where(g => g.MovieId == id)
            .Select(g => g.Genre)
            .ToHashSet();
        var vm = mapper.Map<MovieVm>((movie, genres));

        return vm;
    }

    public int AddMovie(CreateMovieDto movieDto)
    {
        var newMovie = mapper.Map<Movie>(movieDto);
        newMovie.Id = _movies.Count + 1;
        _movies.Add(newMovie);

        return newMovie.Id;
    }


    public void UpdateGenresForMovie(int id, UpdateGenresForMovieDto dto)
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);
        _ = dto.GenreIds.All(CheckIfGenreExistsAndThrowIfNotFound);

        _genresInMovies.RemoveAll(g => g.MovieId == id);
        foreach (var genreId in dto.GenreIds)
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
    }

    public void DeleteMovie(int id)
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);
        _movies.Remove(movie);
        _genresInMovies.RemoveAll(g => g.MovieId == id);
    }

    public void UpdateMovie(int id, UpdateMovieDto dto)
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);

        movie.Title = dto.Title;
        movie.Description = dto.Description;
        movie.DurationInMinutes = dto.DurationInMinutes;
    }

    private Movie GetMovieByIdAndThrowIfNotFound(int id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);
        if (movie is null)
        {
            throw new MovieNotFoundException(id);
        }

        return movie;
    }

    private bool CheckIfGenreExistsAndThrowIfNotFound(int id)
    {
        var genre = _genres.FirstOrDefault(g => g.Id == id);
        if (genre is null)
        {
            throw new GenreNotFoundException(id);
        }

        return true;
    }
}
