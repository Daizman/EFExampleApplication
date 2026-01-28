namespace EFExampleApplication.Contracts;

public record ReviewListVm(ReviewId Id, int Score, string MovieTitle);
public record ListOfReviews(IReadOnlyCollection<ReviewListVm> Reviews);

public record ReviewVm(ReviewId Id, string Content, int Score, string MovieTitle, string ReviewerLogin);

public record CreateReviewDto(string Content, int Score, MovieId MovieId, UserId UserId);

public record UpdateReviewDto(string? Content, int? Score);
