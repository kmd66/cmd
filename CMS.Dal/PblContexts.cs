using CMS.Dal.DbModel;
using Microsoft.EntityFrameworkCore;
using Sample.Model.Data;
using System.Reflection.Metadata;

namespace CMS.Dal
{
    //dotnet tool install --global dotnet-ef --version 7.*
    //dotnet ef migrations add generatedb
    //Add-Migration Update-Database
    public class PblContexts : DbContext
    {
        // public PblContexts(DbContextOptions<PblContexts> options)
        //: base(options)
        // { }add-migration initial

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("pbl");

            modelBuilder.Entity<User>()
                .HasIndex(p => p.UnicId);
            menusConfig(ref modelBuilder);
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
            modelBuilder.Entity<Menu>()
                .Property(b => b.Published).HasDefaultValue(true);

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Menu> Menus { get; set; }
    }
}
