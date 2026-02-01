namespace EFExampleApplication.Contracts.V2;

public record CreateMovieV2Dto(string Title, string? Description, int DurationInMinutes, IReadOnlyCollection<GenreDto> Genres);

public record UpdateMovieV2Dto(string? Title, string? Description, int? DurationInMinutes, IReadOnlyCollection<GenreDto>? Genres);

public record GenreDto(GenreId? Id, string? Name);
