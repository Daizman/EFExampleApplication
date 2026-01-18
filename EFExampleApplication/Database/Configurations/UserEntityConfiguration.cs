using EFExampleApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFExampleApplication.Database.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Login)
            .IsRequired()
            .HasMaxLength(128);
        builder.HasIndex(user => user.Login).IsUnique();

        builder.Property(user => user.Password)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasMany(user => user.Reviews)
            .WithOne(review => review.User)
            .HasForeignKey(review => review.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
