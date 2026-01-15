using AutoMapper;
using EFExampleApplication.Contracts;
using EFExampleApplication.Models;

namespace EFExampleApplication.Configurations.Mappings;

public class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        CreateMap<Movie, MovieVm>()
            .ForCtorParam(nameof(MovieVm.Id), source => source.MapFrom(m => m.Id))
            .ForCtorParam(nameof(MovieVm.Title), source => source.MapFrom(m => m.Title))
            .ForCtorParam(nameof(MovieVm.Description), source => source.MapFrom(m => m.Description))
            .ForCtorParam(nameof(MovieVm.DurationInMinutes), source => source.MapFrom(m => m.DurationInMinutes))
            .ForCtorParam(nameof(MovieVm.Genres), source => source.MapFrom(m => m.Genres.Select(g => g.Genre).ToHashSet()));

        CreateMap<Movie, MovieListVm>();

        CreateMap<IEnumerable<Movie>, ListOfMovies>()
            .ForCtorParam(nameof(ListOfMovies.Movies), source => source.MapFrom(movieList => movieList));

        CreateMap<CreateMovieDto, Movie>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
