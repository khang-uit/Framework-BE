using Memoriesx.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoriesx.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("posts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.CreatorId).IsRequired();
            builder.Property(x => x.SelectedFile).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
        }
    }
}
