using CMS.Dal.DbModel;
using Microsoft.EntityFrameworkCore;
using Sample.Model.Data;
using System.Reflection.Metadata;

namespace CMS.Dal
{
    public class CntContexts : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("cnt");
            postConfig(ref modelBuilder);
            
            modelBuilder
                .Entity<PostDto>(eb =>
                {
                    eb.HasNoKey();
                    eb.ToView("PostViwe");
                });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Register.ConnectionString);

        }

        private void menusConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>()
                .HasIndex(p => p.UnicId);
            modelBuilder.Entity<Menu>()
                .Property(b => b.Link).HasDefaultValue("");
            modelBuilder.Entity<Menu>()
                .Property(b => b.Img).HasDefaultValue("");
            //modelBuilder.Entity<Menu>()
            //    .Property(b => b.Published).HasDefaultValue(true);

            modelBuilder.Ignore<PostDto>();
        }

        private void postConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasIndex(p => p.UnicId);
            modelBuilder.Entity<Post>()
                .Property(b => b.Img).HasDefaultValue("");
            //modelBuilder.Entity<Post>()
            //    .Property(b => b.Published).HasDefaultValue(true);
            //modelBuilder.Entity<Post>()
            //    .Property(b => b.Special).HasDefaultValue(true);
            //modelBuilder.Entity<Post>()
            //    .Property(b => b.Access).HasDefaultValue(1);


        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostDto> PostDtos { get; set; }
    }
}
