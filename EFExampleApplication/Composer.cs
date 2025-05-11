using EFExampleApplication.Abstractions;
using EFExampleApplication.Database;
using EFExampleApplication.Services;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication;

public static class Composer
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Composer).Assembly);
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=EFExampleApplication");
        });
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddControllers();

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();

        return services;
    }
}
