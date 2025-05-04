namespace EFExampleApplication.Models;

/// <summary>
/// Промежуточная модель для связи многие-ко-многим между Movie и Genre
/// </summary>
public class GenreInMovie
{
    public int MovieId { get; set; }
    public required Movie Movie { get; set; }

    public int GenreId { get; set; }
    public required Genre Genre { get; set; }
}
