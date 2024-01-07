using CMS.Dal.DbModel;
using Microsoft.EntityFrameworkCore;
using Sample.Model.Data;

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
            modelBuilder.Entity<Menu>()
                .HasIndex(p => p.UnicId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Register.ConnectionString);

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Menu> Menus { get; set; }
    }
}
