using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialize.Core.Domain.Entities;

namespace Socialize.Infrastructure.Identity.Configuration
{
    internal class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("Friendships", "Domain");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User)
                .WithMany(x => x.Friendships)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Friend)
                .WithMany()
                .HasForeignKey(x => x.FriendId).OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}
