namespace EFExampleApplication.Models;

public class GenreInMovie
{
    public GenreId MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public MovieId GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
}
