using MobileStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using MobileStore.Domain.Migrations;

namespace MobileStore.Domain.DataLayer
{
    public class ApplicationUser : IdentityUser { }
    public class MarketDbContext : IdentityDbContext<ApplicationUser>
    {
        static MarketDbContext()
        {
            Database.SetInitializer<MarketDbContext>(new MigrateDatabaseToLatestVersion<MarketDbContext, Configuration>());
        }

        public MarketDbContext() : base("MarketDbContext") { } 

        public DbSet<Product> Products { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<PostedNew> PostedNews { get; set; }
        public DbSet<DescItem> DescItems { get; set; }
        public DbSet<SliderItem> SliderItems { get; set; }
    }
}
