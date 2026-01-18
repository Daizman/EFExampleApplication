using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFExampleApplication.Database.Configurations;

public class MovieEntityConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(m => m.Description)
            .HasMaxLength(1024);
        builder.Property(m => m.DurationInMinutes)
            .IsRequired();

        builder.HasMany(m => m.Reviews)
            .WithOne(r => r.Movie)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(m => m.GenresForMovie)
            .WithOne(gInM => gInM.Movie)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
