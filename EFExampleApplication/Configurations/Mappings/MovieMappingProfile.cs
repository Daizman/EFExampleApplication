using AutoMapper;
using EFExampleApplication.Contracts;
using EFExampleApplication.Models;

namespace EFExampleApplication.Configurations.Mappings;

public class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        CreateMap<(Movie Movie, HashSet<Genre> Genres), MovieVm>()
            .ForCtorParam(nameof(MovieVm.Id), source => source.MapFrom(m => m.Movie.Id))
            .ForCtorParam(nameof(MovieVm.Title), source => source.MapFrom(m => m.Movie.Title))
            .ForCtorParam(nameof(MovieVm.Description), source => source.MapFrom(m => m.Movie.Description))
            .ForCtorParam(nameof(MovieVm.DurationInMinutes), source => source.MapFrom(m => m.Movie.DurationInMinutes))
            .ForCtorParam(nameof(MovieVm.Genres), source => source.MapFrom(m => m.Genres));

        CreateMap<Movie, MovieListVm>();

        CreateMap<IEnumerable<Movie>, ListOfMovies>()
            .ForCtorParam(nameof(ListOfMovies.Movies), source => source.MapFrom(movieList => movieList));

        CreateMap<CreateMovieDto, Movie>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
