using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFExampleApplication.Database.Configurations;

public class GenreInMovieEntityConfiguration : IEntityTypeConfiguration<GenreInMovie>
{
    public void Configure(EntityTypeBuilder<GenreInMovie> builder)
    {
        builder.HasKey(genreInMovie => new { genreInMovie.MovieId, genreInMovie.GenreId });

        builder.HasOne(genreInMovie => genreInMovie.Genre)
            .WithMany(genre => genre.MoviesForGenre)
            .HasForeignKey(genreInMovie => genreInMovie.GenreId);

        builder.HasOne(genreInMovie => genreInMovie.Movie)
            .WithMany(movie => movie.GenresForMovie)
            .HasForeignKey(genreInMovie => genreInMovie.MovieId);
    }
}
