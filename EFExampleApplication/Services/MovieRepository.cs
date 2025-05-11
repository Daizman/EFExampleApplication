using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Database;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;

namespace EFExampleApplication.Services;

public class MovieRepository(
    IMapper mapper,
    ApplicationDbContext dbContext
) : IMovieRepository
{
    public ListOfMovies GetMovies() => mapper.Map<ListOfMovies>(dbContext.Movies.ToList());

    public MovieVm GetMovie(int id)
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);

        var genres = dbContext.GenreInMovies
          .Where(g => g.MovieId == id)
          .Select(g => g.Genre)
          .ToHashSet();

        var vm = mapper.Map<MovieVm>((movie, genres));

        return vm;
    }

    public int AddMovie(CreateMovieDto movieDto)
    {
        var newMovie = mapper.Map<Movie>(movieDto);
        dbContext.Add(newMovie);

        dbContext.SaveChanges();

        return newMovie.Id;
    }

    public void UpdateGenresForMovie(int id, UpdateGenresForMovieDto dto)
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);

        var genreById = dbContext.Genres
          .Where(g => dto.GenreIds.Contains(g.Id))
          .ToDictionary(g => g.Id, g => g);

        var firstNotFoundGenre = dto.GenreIds.FirstOrDefault(id => !genreById.ContainsKey(id));
        if (firstNotFoundGenre != 0)
        {
            throw new GenreNotFoundException(firstNotFoundGenre);
        }

        dbContext.RemoveRange(
          dbContext.GenreInMovies.Where(g => g.MovieId == id)
        );

        var genresInMovie = new List<GenreInMovie>();
        foreach (var genreId in dto.GenreIds)
        {
            var genreInMovie = new GenreInMovie
            {
                MovieId = movie.Id,
                Movie = movie,
                GenreId = genreId,
                Genre = genreById[genreId],
            };
            genresInMovie.Add(genreInMovie);
        }
        dbContext.AddRange(genresInMovie);

        dbContext.SaveChanges();
    }

    public void UpdateMovie(int id, UpdateMovieDto dto)
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);
        movie.Title = dto.Title;
        movie.Description = dto.Description;
        movie.DurationInMinutes = dto.DurationInMinutes;

        dbContext.SaveChanges();
    }

    public void DeleteMovie(int id)
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);
        dbContext.Remove(movie);

        dbContext.SaveChanges();
    }

    private Movie GetMovieByIdAndThrowIfNotFound(int id)
    {
        var movie = dbContext.Movies.FirstOrDefault(m => m.Id == id);
        if (movie is null)
        {
            throw new MovieNotFoundException(id);
        }

        return movie;
    }
}