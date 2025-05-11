using AutoMapper;
using EFExampleApplication.Contracts;
using EFExampleApplication.Models;

namespace EFExampleApplication.Configurations.Mappings;

public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<(MovieVm Movie, UserVm User, Review Review), ReviewVm>()
            .ForCtorParam(nameof(ReviewVm.Id), opt => opt.MapFrom(src => src.Review.Id))
            .ForCtorParam(nameof(ReviewVm.Content), opt => opt.MapFrom(src => src.Review.Content))
            .ForCtorParam(nameof(ReviewVm.Score), opt => opt.MapFrom(src => src.Review.Score))
            .ForCtorParam(nameof(ReviewVm.MovieTitle), opt => opt.MapFrom(src => src.Movie.Title))
            .ForCtorParam(nameof(ReviewVm.ReviewerLogin), opt => opt.MapFrom(src => src.User.Login));

        CreateMap<(MovieVm Movie, List<Review> Reviews), ListOfReviews>()
            .ForCtorParam(
                nameof(ListOfReviews.Reviews),
                source => source
                    .MapFrom(reviewList
                        => reviewList
                            .Reviews
                            .Select(review => new ReviewListVm(review.Id, review.Score, reviewList.Movie.Title))
                            .ToHashSet()));

        CreateMap<CreateReviewDto, Review>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
