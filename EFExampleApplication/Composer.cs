using EFExampleApplication.Abstractions;
using EFExampleApplication.Database;
using EFExampleApplication.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication;

public static class Composer
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddAutoMapper(typeof(Composer).Assembly);
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(
                "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=EFExampleApplication"
            );

            if (environment.IsDevelopment())
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                options
                    .UseLoggerFactory(loggerFactory)
                    .LogTo(
                        msg => serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>().LogInformation(msg),
                        [DbLoggerCategory.Database.Command.Name],
                        LogLevel.Information);
            }
        });
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddControllers();
        services.AddLogging();

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IReviewService, ReviewService>();

        return services;
    }

    private static IServiceCollection AddLogging(this IServiceCollection services)
    {
        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.RequestMethod
                                    | HttpLoggingFields.RequestPath
                                    | HttpLoggingFields.RequestQuery
                                    | HttpLoggingFields.RequestHeaders
                                    | HttpLoggingFields.RequestBody
                                    | HttpLoggingFields.ResponseStatusCode
                                    | HttpLoggingFields.ResponseHeaders
                                    | HttpLoggingFields.ResponseBody
                                    | HttpLoggingFields.Duration;

            logging.CombineLogs = true;
        });

        return services;
    }
}
