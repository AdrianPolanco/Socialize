

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialize.Core.Domain.Entities;

namespace Socialize.Infrastructure.Identity.Configuration
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments", "Domain");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired().HasColumnType("text");
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.HasMany(x => x.Replies)
                .WithOne(x => x.ParentComment)
                .HasForeignKey(x => x.ParentCommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}
