namespace EFExampleApplication.Contracts;

public record MovieListVm(MovieId Id, string Title);
public record ListOfMovies(IReadOnlyCollection<MovieListVm> Movies);

public record GenreVm(GenreId Id, string Name);
public record MovieVm(MovieId Id, string Title, string? Description, int DurationInMinutes, IReadOnlyCollection<GenreVm> Genres);

public record CreateMovieDto(string Title, string? Description, int DurationInMinutes);

public record UpdateGenresForMovieDto(GenreId[] GenreIds);

public record UpdateMovieDto(string? Title, string? Description, int? DurationInMinutes);
