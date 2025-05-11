using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFExampleApplication.Database;

public class GenreEntityConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(genre => genre.Id);
        builder.HasData(
            new() { Id = 1, Name = "Action" },
            new() { Id = 2, Name = "Comedy" },
            new() { Id = 3, Name = "Drama" },
            new() { Id = 4, Name = "Horror" },
            new() { Id = 5, Name = "Sci-Fi" }
        );
    }
}
