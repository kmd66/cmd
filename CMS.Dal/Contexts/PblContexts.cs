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
            orderConfig(ref modelBuilder);


            modelBuilder
                .Entity<Status>(eb =>
                {
                    eb.HasNoKey();
                    eb.ToView("StatusViwe");
                });

            modelBuilder
                .Entity<StatusType>(eb =>
                {
                    eb.HasNoKey();
                    eb.ToView("StatusTypeViwe");
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
        }

        private void orderConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasIndex(p => p.UnicId);
            modelBuilder.Entity<Order>()
                .Property(b => b.Text).HasDefaultValue("");
            modelBuilder
                .Entity<OrderPost>(eb =>
                {
                    eb.HasNoKey();
                    eb.ToView("OrderPostViwe");
                });
            modelBuilder
                .Entity<OrderDto>(eb =>
                {
                    eb.HasNoKey();
                    eb.ToView("OrderDtoViwe");
                });
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderPost> OrderPost { get; set; }
        public DbSet<OrderDto> OrderDto { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<StatusType> StatusTypes { get; set; }
    }
}
