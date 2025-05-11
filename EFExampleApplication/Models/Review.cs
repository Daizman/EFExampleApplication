namespace EFExampleApplication.Models;

public class Review
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public int Score { get; set; }

    public int MovieId { get; set; }
    public required virtual Movie Movie { get; set; }
    public int UserId { get; set; }
    public required virtual User User { get; set; }
}
