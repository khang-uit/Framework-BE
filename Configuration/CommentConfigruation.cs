using Memoriesx.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoriesx.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PostId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Content).IsRequired();
        }
    }
}
