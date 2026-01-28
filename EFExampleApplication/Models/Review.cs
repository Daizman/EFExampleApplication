namespace EFExampleApplication.Models;

public class Review
{
    public ReviewId Id { get; set; }
    public required string Content { get; set; }
    public int Score { get; set; }

    public MovieId MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public UserId UserId { get; set; }
    public User User { get; set; } = null!;
}
