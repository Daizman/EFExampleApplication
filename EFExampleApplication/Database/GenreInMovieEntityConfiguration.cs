using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFExampleApplication.Database;

public class GenreInMovieEntityConfiguration : IEntityTypeConfiguration<GenreInMovie>
{
    public void Configure(EntityTypeBuilder<GenreInMovie> builder)
    {
        builder.HasKey(gm => new { gm.MovieId, gm.GenreId });

        builder.HasOne(gm => gm.Movie)
            .WithMany(m => m.Genres)
            .HasForeignKey(gm => gm.MovieId)
            .HasPrincipalKey(m => m.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gm => gm.Genre)
            .WithMany(g => g.Movies)
            .HasForeignKey(gm => gm.GenreId)
            .HasPrincipalKey(g => g.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
