using AutoMapper;
using EFExampleApplication.Contracts;
using EFExampleApplication.Models;

namespace EFExampleApplication.Configurations.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserVm>()
            .ForCtorParam(nameof(UserVm.Id), source => source.MapFrom(u => u.Id))
            .ForCtorParam(nameof(UserVm.Login), source => source.MapFrom(u => u.Login));

        CreateMap<User, UserListVm>()
            .ForCtorParam(nameof(UserListVm.Id), source => source.MapFrom(u => u.Id))
            .ForCtorParam(nameof(UserListVm.Login), source => source.MapFrom(u => u.Login));

        CreateMap<ICollection<User>, ListOfUsers>()
            .ForCtorParam(nameof(ListOfUsers.Users), source => source.MapFrom(userList => userList));

        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}