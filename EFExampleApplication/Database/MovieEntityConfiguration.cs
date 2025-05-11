using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFExampleApplication.Database;

public class MovieEntityConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(movie => movie.Id);
        builder.HasMany(movie => movie.Reviews)
            .WithOne(review => review.Movie)
            .HasForeignKey(review => review.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
