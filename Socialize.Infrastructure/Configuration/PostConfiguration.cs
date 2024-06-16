using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialize.Core.Domain.Entities;

namespace Socialize.Infrastructure.Identity.Configuration
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts", "Domain");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired().HasColumnType("text");
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.HasMany(x => x.Comments)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Attachment)
                .WithMany()
                .HasForeignKey(x => x.AttachmentId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}
