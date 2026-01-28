using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Exceptions;
using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Services;

public class MovieService : IMovieService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public MovieService(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public MovieVm GetMovie(MovieId id)
    {
        var movie = _applicationDbContext.Movies
            .AsNoTracking()
            .Where(m => m.Id == id)
            .ProjectTo<MovieVm>(_mapper.ConfigurationProvider)
            .FirstOrDefault();

        if (movie is null)
        {
            throw new MovieNotFoundException(id);
        }

        return movie;
    }

    public ListOfMovies GetMovies()
    {
        var movies = _applicationDbContext.Movies.AsNoTracking()
            .Select(m => new MovieListVm(m.Id, m.Title))
            .ToList();

        return new ListOfMovies(movies);
    }

    public int AddMovie(CreateMovieDto movieDto)
    {
        var newMovie = _mapper.Map<Movie>(movieDto);
        _applicationDbContext.Movies.Add(newMovie);

        _applicationDbContext.SaveChanges();

        return newMovie.Id;
    }

    public void DeleteMovie(MovieId id)
    {
        var deleted = _applicationDbContext.Movies
            .Where(movie => movie.Id == id)
            .ExecuteDelete();
        if (deleted == 0)
        {
            throw new MovieNotFoundException(id);
        }
    }

    public void UpdateGenresForMovie(MovieId id, UpdateGenresForMovieDto dto)
    {
        var movieExists = _applicationDbContext
            .Movies
            .Any(m => m.Id == id);

        if (!movieExists)
        {
            throw new MovieNotFoundException(id);
        }

        var newGenreIds = _applicationDbContext.Genres
            .AsNoTracking()
            .Where(g => dto.GenreIds.Contains(g.Id))
            .Select(g => g.Id)
            .ToList();

        if (newGenreIds.Count != dto.GenreIds.Length)
        {
            throw new GenreNotFoundException();
        }

        var existingGenres = _applicationDbContext.GenreInMovies.Where(gInM => gInM.MovieId == id);
        _applicationDbContext.GenreInMovies.RemoveRange(existingGenres);

        var newGenres = newGenreIds.Select(genreId => 
            new GenreInMovie
            {
                MovieId = id,
                GenreId = genreId,
            }).ToList();
        _applicationDbContext.GenreInMovies.AddRange(newGenres);
        _applicationDbContext.SaveChanges();
    }

    public void UpdateMovie(MovieId id, UpdateMovieDto dto)
    {
        var updated = _applicationDbContext.Movies
            .Where(m => m.Id == id)
            .ExecuteUpdate(setters => setters
                .SetProperty(m => m.Title, m => dto.Title ?? m.Title)
                .SetProperty(m => m.Description, m => dto.Description ?? m.Description)
                .SetProperty(m => m.DurationInMinutes, m => dto.DurationInMinutes ?? m.DurationInMinutes)
            );

        if (updated == 0) throw new MovieNotFoundException(id);
    }
}
