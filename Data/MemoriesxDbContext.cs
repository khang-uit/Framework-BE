using Memoriesx.Configuration;
using Memoriesx.Models;
using Memoriesx.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Memoriesx.Data
{
    public class MemoriesxDbContext: DbContext
    {
        public MemoriesxDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new LikeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfiguration());
            modelBuilder.ApplyConfiguration(new ViewConfiguration());

            modelBuilder.Entity<Like>()
                .HasKey(bc => new { bc.UserId, bc.PostId, bc.Id });
            modelBuilder.Entity<Like>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.Likes)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<Like>()
                .HasOne(bc => bc.Post)
                .WithMany(c => c.Likes)
                .HasForeignKey(bc => bc.PostId);

            modelBuilder.Entity<Comment>()
                .HasKey(bc => new { bc.UserId, bc.PostId, bc.Id });
            modelBuilder.Entity<Comment>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.Comments)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<Comment>()
                .HasOne(bc => bc.Post)
                .WithMany(c => c.Comments)
                .HasForeignKey(bc => bc.PostId);

            modelBuilder.Entity<Report>()
                .HasKey(bc => new { bc.CreatorId, bc.ReportedId, bc.PostId, bc.Id });
            modelBuilder.Entity<Report>()
                .HasOne(bc => bc.Creator)
                .WithMany(b => b.Creators)
                .HasForeignKey(bc => bc.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Report>()
                .HasOne(bc => bc.Reported)
                .WithMany(c => c.Reporteds)
                .HasForeignKey(bc => bc.ReportedId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Report>()
                .HasOne(bc => bc.Post)
                .WithMany(c => c.Reports)
                .HasForeignKey(bc => bc.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Seed();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<View> Views { get; set; }
    }
}
