namespace Pañol.Migrations
{
    using Pañol.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Pañol.Data.PañolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Pañol.Data.PañolContext context)
        {
            context.Roles.AddOrUpdate(
             r => r.Id,
             new Rol { Id = 1, RolNombre = "Admin" },
                new Rol { Id = 2, RolNombre = "User" });

           



        }
    }
}
