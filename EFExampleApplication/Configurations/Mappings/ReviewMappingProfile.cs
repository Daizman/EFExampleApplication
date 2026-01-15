using AutoMapper;
using EFExampleApplication.Contracts;
using EFExampleApplication.Models;

namespace EFExampleApplication.Configurations.Mappings;

public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<Review, ReviewVm>()
            .ForCtorParam(nameof(ReviewVm.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(ReviewVm.Content), opt => opt.MapFrom(src => src.Content))
            .ForCtorParam(nameof(ReviewVm.Score), opt => opt.MapFrom(src => src.Score))
            .ForCtorParam(nameof(ReviewVm.MovieTitle), opt => opt.MapFrom(src => src.Movie.Title))
            .ForCtorParam(nameof(ReviewVm.ReviewerLogin), opt => opt.MapFrom(src => src.User.Login));

        CreateMap<IEnumerable<Review>, ListOfReviews>()
            .ForCtorParam(
                nameof(ListOfReviews.Reviews),
                source => source
                    .MapFrom(reviewList
                        => reviewList
                            .Select(review => new ReviewListVm(review.Id, review.Score, review.Movie.Title))
                            .ToHashSet()));

        CreateMap<CreateReviewDto, Review>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
