

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialize.Core.Domain.Entities;

namespace Socialize.Infrastructure.Identity.Configuration
{
    internal class ReplyConfiguration : IEntityTypeConfiguration<Reply>
    {
        public void Configure(EntityTypeBuilder<Reply> builder)
        {
            builder.ToTable("Replies", "Domain");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired().HasColumnType("text");
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}
