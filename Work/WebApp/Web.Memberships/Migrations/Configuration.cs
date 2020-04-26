namespace Web.Memberships.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Web.Memberships.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            //don't make it true in production environment. Fir the purpose of this tutorial making it true
            AutomaticMigrationsEnabled = true;
            //in production environment don't make the following true 
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Web.Memberships.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
