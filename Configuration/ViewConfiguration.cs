using Memoriesx.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Memoriesx.Configuration
{
    public class ViewConfiguration : IEntityTypeConfiguration<View>
    {
        public void Configure(EntityTypeBuilder<View> builder)
        {
            builder.ToTable("views");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Count).IsRequired();
            builder.Property(x => x.Date).IsRequired();
        }
    }
}
