using AutoMapper;
using EFExampleApplication.Configurations.Mappings;
using EFExampleApplication.Database;
using Microsoft.Data.Sqlite;

namespace EFExampleApplication.Tests;

public abstract class TestServiceBase : IDisposable
{
    protected readonly ApplicationDbContext Context;
    protected readonly IMapper Mapper;
    private readonly SqliteConnection _connection;

    protected TestServiceBase()
    {
        (Context, _connection) = FakeApplicationDbContextFactory.Create();

        var config = new MapperConfiguration(cfg =>
            cfg.AddMaps(typeof(ReviewMappingProfile).Assembly));
        Mapper = config.CreateMapper();
    }

    public void Dispose()
    {
        FakeApplicationDbContextFactory.Destroy(Context, _connection);
    }
}
