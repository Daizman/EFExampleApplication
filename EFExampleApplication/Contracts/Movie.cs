namespace EFExampleApplication.Contracts;

public record MovieListVm(int Id, string Title);
public record ListOfMovies(IReadOnlySet<MovieListVm> Movies);

public record GenreVm(int Id, string Name);
public record MovieVm(int Id, string Title, string? Description, int DurationInMinutes, IReadOnlySet<GenreVm> Genres);

public record CreateMovieDto(string Title, string? Description, int DurationInMinutes, MovieGenreDto[] Genres);
public record MovieGenreDto(int? Id, string? Name);

public record UpdateGenresForMovieDto(MovieGenreDto[] Genres);

public record UpdateMovieDto(string Title, string? Description, int DurationInMinutes);
