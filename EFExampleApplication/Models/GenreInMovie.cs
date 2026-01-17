namespace EFExampleApplication.Models;

public class GenreInMovie
{
    public int MovieId { get; set; }
    public virtual required Movie Movie { get; set; }

    public int GenreId { get; set; }
    public virtual required Genre Genre { get; set; }
}
