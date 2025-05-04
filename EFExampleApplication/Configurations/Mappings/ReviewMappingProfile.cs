using AutoMapper;
using EFExampleApplication.Contracts;
using EFExampleApplication.Models;

namespace EFExampleApplication.Configurations.Mappings;

public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<Review, ReviewVm>();

        CreateMap<Review, ReviewListVm>();

        CreateMap<IEnumerable<Review>, ListOfReviews>()
            .ForCtorParam(nameof(ListOfReviews.Reviews), source => source.MapFrom(reviewList => reviewList));

        CreateMap<CreateReviewDto, Review>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
