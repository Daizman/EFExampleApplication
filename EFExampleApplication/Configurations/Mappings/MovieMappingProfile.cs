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
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(dto => dto.Genres));
        CreateMap<CreateMovieDto, GenreInMovie>()
            .ForMember(dest => dest.MovieId, opt => opt.Ignore())
            .ForMember(dest => dest.Movie, opt => opt.MapFrom(dto => dto));
        CreateMap<MovieGenreDto, Genre>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(dto => dto.Id ?? default));
        CreateMap<MovieGenreDto, GenreInMovie>()
            .ForMember(dest => dest.GenreId, opt => opt.MapFrom(dto => dto.Id ?? default))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(dto => dto));
    }
}
