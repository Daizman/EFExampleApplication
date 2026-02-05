using AutoMapper;
using EFExampleApplication.Contracts;
using EFExampleApplication.Contracts.V2;
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
            .ForCtorParam(nameof(MovieVm.Genres), source => source.MapFrom(m => m.GenresForMovie.Select(g => g.Genre)));

        CreateMap<Movie, MovieListVm>();

        CreateMap<ICollection<Movie>, ListOfMovies>()
            .ForCtorParam(nameof(ListOfMovies.Movies), source => source.MapFrom(movieList => movieList));

        CreateMap<CreateMovieDto, Movie>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore())
            .ForMember(dest => dest.GenresForMovie, opt => opt.Ignore());
        CreateMap<CreateMovieV2Dto, Movie>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore())
            .ForMember(dest => dest.GenresForMovie, opt => opt.Ignore());
    }
}
