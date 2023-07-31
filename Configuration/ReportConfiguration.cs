using Memoriesx.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoriesx.Configuration
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("reports");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatorId);
            builder.Property(x => x.PostId);
            builder.Property(x => x.ReportedId);
            builder.Property(x => x.Message).IsRequired();
        }
    }
}
