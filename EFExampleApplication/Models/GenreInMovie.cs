namespace EFExampleApplication.Models;

public class GenreInMovie
{
    public int MovieId { get; set; }
    public required Movie Movie { get; set; }

    public int GenreId { get; set; }
    public required Genre Genre { get; set; }
}
