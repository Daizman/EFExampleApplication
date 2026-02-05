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
    private readonly ILogger<MovieService> _logger;

    public MovieService(
        IApplicationDbContext applicationDbContext, 
        IMapper mapper,
        ILogger<MovieService> logger
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _logger = logger;
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
            _logger.LogError("Couldn't get movie with {Id}. Not found.", id);
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

        _logger.LogInformation("Successfully added movie with {Id}", movie.Id);

        return movie.Id;
    }

    public int AddMovie(CreateMovieV2Dto dto)
    {
        var movie = _mapper.Map<Movie>(dto);

        UpdateGenresForMovie(movie, dto.Genres);

        _applicationDbContext.Movies.Add(movie);

        _applicationDbContext.SaveChanges();

        _logger.LogInformation("Successfully added movie with {Id}", movie.Id);

        return movie.Id;
    }

    public void DeleteMovie(MovieId id)
    {
        var deleted = _applicationDbContext.Movies
            .Where(movie => movie.Id == id)
            .ExecuteDelete();
        if (deleted == 0)
        {
            _logger.LogError("Couldn't delete movie with {Id}. Not found.", id);
            throw new MovieNotFoundException(id);
        }

        _logger.LogInformation("Successfully deleted movie with {Id}", id);
    }

    public void UpdateGenresForMovie(MovieId id, UpdateGenresForMovieDto dto)
    {
        var movieExists = _applicationDbContext
            .Movies
            .Any(m => m.Id == id);

        if (!movieExists)
        {
            _logger.LogError("Couldn't change genres for movie {Id}. Not found.", id);
            throw new MovieNotFoundException(id);
        }

        var newGenreIds = _applicationDbContext.Genres
            .AsNoTracking()
            .Where(g => dto.GenreIds.Contains(g.Id))
            .Select(g => g.Id)
            .ToList();

        if (newGenreIds.Count != dto.GenreIds.Length)
        {
            _logger.LogError("There are not existsing genre in new genres for movie {Id}", id);
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

        _logger.LogInformation("Successfully update genres for movie with {Id}", id);
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

        if (updated == 0)
        {
            _logger.LogError("Couldn't update movie with {Id}. Not found.", id);
            throw new MovieNotFoundException(id);
        }

        _logger.LogInformation("Successfully update movie with {Id}", id);
    }

    public void UpdateMovie(int id, UpdateMovieV2Dto dto)
    {
        var movie = _applicationDbContext.Movies
            .Include(m => m.GenresForMovie)
            .FirstOrDefault(m => m.Id == id);
        if (movie is null)
        {
            _logger.LogError("Couldn't update movie with {Id}. Not found.", id);
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

        _logger.LogInformation("Successfully update movie with {Id}", id);
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
                    _logger.LogError("There are not existsing genre {GenreId} in new genres for movie {MovieId}", 
                        genreDto.Id.Value, 
                        movie.Id);
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
        _logger.LogDebug("Obtaining genres from the database: {@Genres}", genres);
        var genreIds = genres.Where(g => g.Id.HasValue).Select(g => g.Id!.Value);
        var genreById = _applicationDbContext.Genres
          .Where(g => genreIds.Contains(g.Id))
          .ToDictionary(g => g.Id, g => g);

        return genreById;
    }
}
