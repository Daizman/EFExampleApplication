namespace EFExampleApplication.Models;

public class Review
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public int Score { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
