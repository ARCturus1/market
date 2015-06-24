using MobileStore.Domain.Entities;

namespace MobileStore.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MobileStore.Domain.DataLayer.MarketDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MobileStore.Domain.DataLayer.MarketDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //context.PostedNews.AddOrUpdate(n => n.Name,
            //    new PostedNew()
            //    {
            //        Name = "New 1 the best new in world!",
            //        Description = "The new about the best thinks in my own live and more, more, more..."
            //    }, new PostedNew()
            //    {
            //        Name = "New 2 the best new in world!",
            //        Description = "The new about the best thinks in my own live and more, more, more..."
            //    });
        }
    }
}
