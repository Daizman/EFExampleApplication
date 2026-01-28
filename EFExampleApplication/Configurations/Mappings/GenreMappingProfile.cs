using AutoMapper;
using EFExampleApplication.Contracts;
using EFExampleApplication.Models;

namespace EFExampleApplication.Configurations.Mappings;

public class GenreMappingProfile : Profile
{
    public GenreMappingProfile()
    {
        CreateMap<Genre, GenreVm>()
            .ForCtorParam(nameof(GenreVm.Id), source => source.MapFrom(g => g.Id))
            .ForCtorParam(nameof(GenreVm.Name), source => source.MapFrom(g => g.Name));
    }
}
