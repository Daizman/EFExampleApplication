using AutoMapper;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Database;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

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

        ExecuteWithSave(() => dbContext.Add(newMovie));

        return newMovie.Id;
    }


    public void UpdateGenresForMovie(int id, UpdateGenresForMovieDto dto) => ExecuteWithSave(() =>
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);
        CheckIfGenresExistsAndThrowIfNotFound(dto.GenreIds);

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
                Genre = dbContext.Genres.First(g => g.Id == genreId),
            };
            genresInMovie.Add(genreInMovie);
        }

        dbContext.AddRange(genresInMovie);
    });

    public void UpdateMovie(int id, UpdateMovieDto dto) => ExecuteWithSave(() =>
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);

        movie.Title = dto.Title;
        movie.Description = dto.Description;
        movie.DurationInMinutes = dto.DurationInMinutes;
    });

    public void DeleteMovie(int id) => ExecuteWithSave(() =>
    {
        var movie = GetMovieByIdAndThrowIfNotFound(id);
        dbContext.GenreInMovies.RemoveRange(
            dbContext.GenreInMovies.Where(g => g.MovieId == id)
        );
        dbContext.Remove(movie);
    });

    private Movie GetMovieByIdAndThrowIfNotFound(int id)
    {
        var movie = dbContext.Movies.FirstOrDefault(m => m.Id == id);
        if (movie is null)
        {
            throw new MovieNotFoundException(id);
        }

        return movie;
    }

    private void CheckIfGenresExistsAndThrowIfNotFound(int[] ids)
    {
        var dbGenresForCheck = dbContext
            .Genres
            .Where(g => ids.Contains(g.Id))
            .Select(g => g.Id)
            .ToHashSet();
        var firstNotFoundGenre = ids.FirstOrDefault(id => !dbGenresForCheck.Contains(id));
        if (firstNotFoundGenre != 0)
        {
            throw new GenreNotFoundException(firstNotFoundGenre);
        }
    }

    private void ExecuteWithSave(Action action)
    {
        try
        {
            action();
            dbContext.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            throw new DbUpdateException("Database update error occurred", ex);
        }
    }
}
