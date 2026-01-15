using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFExampleApplication.Database;

public class GenreInMovieEntityConfiguration : IEntityTypeConfiguration<GenreInMovie>
{
    public void Configure(EntityTypeBuilder<GenreInMovie> builder)
    {
        builder.HasKey(gm => new { gm.MovieId, gm.GenreId });

        builder.HasOne(gInM => gInM.Genre)
            .WithMany(g => g.Movies)
            .HasForeignKey(gInM => gInM.GenreId);
        builder.HasOne(gInM => gInM.Movie)
            .WithMany(m => m.Genres)
            .HasForeignKey(gInM => gInM.MovieId);
    }
}