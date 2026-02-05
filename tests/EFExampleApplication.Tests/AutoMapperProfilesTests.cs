using AutoMapper;
using EFExampleApplication.Configurations.Mappings;

namespace EFExampleApplication.Tests;

public class AutoMapperProfilesTests
{
    [Fact]
    public void ProfilesConfiguration_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GenreMappingProfile>();
            cfg.AddProfile<MovieMappingProfile>();
            cfg.AddProfile<ReviewMappingProfile>();
            cfg.AddProfile<UserMappingProfile>();
        });

        config.AssertConfigurationIsValid();
    }
}
