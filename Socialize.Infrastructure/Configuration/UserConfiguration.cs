using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialize.Core.Domain.Entities;

namespace Socialize.Infrastructure.Identity.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "Domain"); 
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Lastname).IsRequired().HasMaxLength(100);
            builder.OwnsOne(x => x.Email, y =>
            {
                y.Property(x => x.Value).HasColumnName("Email").IsRequired().HasMaxLength(100);
            });
            builder.OwnsOne(x => x.PhoneNumber, y =>
            {
                y.Property(x => x.Value).HasColumnName("PhoneNumber").IsRequired().HasMaxLength(12);
            });
            builder.Property(x => x.IsActived).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Password);

            builder.HasMany(x => x.Friendships)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Posts)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Comments)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Replies)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}
