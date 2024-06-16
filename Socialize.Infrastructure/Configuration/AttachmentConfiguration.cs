using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialize.Core.Domain.Entities;

namespace Socialize.Infrastructure.Identity.Configuration
{
    internal class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.ToTable("Attachments", "Domain");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Url).IsRequired().HasMaxLength(350);
            builder.Property(x => x.Type).HasConversion<int>().IsRequired();
            builder.HasOne(x => x.Post)
                .WithOne(x => x.Attachment)
                .HasForeignKey<Attachment>(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}
