using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFExampleApplication.Database.Configurations;

public class GenreEntityConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(genre => genre.Id);
        builder.Property(genre => genre.Name)
          .IsRequired()
          .HasMaxLength(128);
        builder.HasIndex(genre => genre.Name).IsUnique();

        builder.HasData(
          new() { Id = 1, Name = "Action" },
          new() { Id = 2, Name = "Comedy" },
          new() { Id = 3, Name = "Drama" },
          new() { Id = 4, Name = "Horror" },
          new() { Id = 5, Name = "Sci-Fi" }
        );

        builder.HasMany(genre => genre.MoviesForGenre)
            .WithOne(genreInMovie => genreInMovie.Genre)
            .HasForeignKey(genreInMovie => genreInMovie.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
