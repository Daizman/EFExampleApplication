using EFExampleApplication.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EFExampleApplication.Tests;

public static class FakeApplicationDbContextFactory
{
    public const int UserAId = 1;
    public const int UserBId = 2;

    public const int MovieId = 1;

    public const int ReviewIdToRead = 1;
    public const int ReviewIdToUpdate = 2;
    public const int ReviewIdToDelete = 3;

    public static (ApplicationDbContext context, SqliteConnection connection) Create()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.AddRange(
            new Models.User
            {
                Id = UserAId,
                Login = "A",
                Password = "Pass",
            },
            new Models.User
            {
                Id = UserBId,
                Login = "B",
                Password = "Pass",
            });

        context.Movies.Add(
            new Models.Movie
            {
                Id = MovieId,
                Title = "Movie",
                DurationInMinutes = 120,
            });

        context.Reviews.AddRange(
            new Models.Review
            {
                Id = ReviewIdToRead,
                Content = "Content1",
                MovieId = MovieId,
                Score = 1,
                UserId = UserAId,
            },
            new Models.Review
            {
                Id = ReviewIdToDelete,
                Content = "ToDelete",
                MovieId = MovieId,
                Score = 5,
                UserId = UserAId,
            },
            new Models.Review
            {
                Id = ReviewIdToUpdate,
                Content = "ToUpdate",
                MovieId = MovieId,
                Score = 3,
                UserId = UserBId,
            });

        context.SaveChanges();
        context.ChangeTracker.Clear();

        return (context, connection);
    }

    public static void Destroy(ApplicationDbContext context, SqliteConnection connection)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
        connection.Dispose();
    }
}
