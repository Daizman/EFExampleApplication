using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFExampleApplication.Abstractions;
using EFExampleApplication.Contracts;
using EFExampleApplication.Contracts.V2;
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

    public int AddMovie(CreateMovieDto dto)
    {
        var movie = _mapper.Map<Movie>(dto);
        _applicationDbContext.Movies.Add(movie);

        _applicationDbContext.SaveChanges();

        return movie.Id;
    }

    public int AddMovie(CreateMovieV2Dto dto)
    {
        var movie = _mapper.Map<Movie>(dto);

        UpdateGenresForMovie(movie, dto.Genres);

        _applicationDbContext.Movies.Add(movie);

        _applicationDbContext.SaveChanges();

        return movie.Id;
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

    public void UpdateMovie(int id, UpdateMovieV2Dto dto)
    {
        var movie = _applicationDbContext.Movies
            .Include(m => m.GenresForMovie)
            .FirstOrDefault(m => m.Id == id);
        if (movie is null)
        {
            throw new MovieNotFoundException(id);
        }

        if (dto.Genres is not null)
        {
            UpdateGenresForMovie(movie, dto.Genres);
        }

        movie.Title = dto.Title ?? movie.Title;
        movie.Description = dto.Description ?? movie.Description;
        movie.DurationInMinutes = dto.DurationInMinutes ?? movie.DurationInMinutes;

        _applicationDbContext.SaveChanges();
    }

    private void UpdateGenresForMovie(Movie movie, IReadOnlyCollection<GenreDto> genres)
    {
        movie.GenresForMovie.Clear();
        var genreById = GetGenresDictionary(genres);
        foreach (var genreDto in genres)
        {
            if (genreDto.Id.HasValue)
            {
                if (!genreById.TryGetValue(genreDto.Id.Value, out var existsingGenre))
                {
                    throw new GenreNotFoundException(genreDto.Id.Value);
                }
                movie.GenresForMovie.Add(new GenreInMovie
                {
                    GenreId = existsingGenre.Id,
                    Genre = existsingGenre,
                });
                continue;
            }
            if (genreDto.Name is not null)
            {
                movie.GenresForMovie.Add(new GenreInMovie
                {
                    Genre = new Genre { Name = genreDto.Name },
                });
            }
        }
    }

    private Dictionary<GenreId, Genre> GetGenresDictionary(IReadOnlyCollection<GenreDto> genres)
    {
        var genreIds = genres.Where(g => g.Id.HasValue).Select(g => g.Id!.Value);
        var genreById = _applicationDbContext.Genres
          .Where(g => genreIds.Contains(g.Id))
          .ToDictionary(g => g.Id, g => g);

        return genreById;
    }
}
