namespace EFExampleApplication.Models;

public class Genre
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<GenreInMovie> MoviesForGenre { get; set; } = [];
}
