namespace EFExampleApplication.Contracts;

public record ReviewListVm(int Id, int Score, string MovieTitle);
public record ListOfReviews(IReadOnlySet<ReviewListVm> Reviews);

public record ReviewVm(int Id, string Content, int Score, string MovieTitle, string ReviewerLogin);

public record CreateReviewDto(string Content, int Score, int MovieId, int UserId);

public record UpdateReviewDto(string Content, int Score);
