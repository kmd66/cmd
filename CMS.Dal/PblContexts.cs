using CMS.Dal.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CMS.Model.Property.ConnectionString);

        }
        public DbSet<User> Users { get; set; }
    }
}
