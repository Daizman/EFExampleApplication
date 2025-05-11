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

        CreateMap<Review, ReviewListVm>()
            .ForCtorParam(nameof(ReviewListVm.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(ReviewListVm.Score), opt => opt.MapFrom(src => src.Score))
            .ForCtorParam(nameof(ReviewListVm.MovieTitle), opt => opt.MapFrom(src => src.Movie.Title));

        CreateMap<List<Review>, ListOfReviews>()
            .ForCtorParam(nameof(ListOfReviews.Reviews), opt => opt.MapFrom(reviewList => reviewList));

        CreateMap<CreateReviewDto, Review>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
