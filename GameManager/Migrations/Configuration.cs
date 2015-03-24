namespace GameManager.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Helpers;
    using GameManager.Helpers;
    using GameManager.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<GameManager.DAL.GameManagerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GameManager.DAL.GameManagerContext context)
        {
            var users = new List<User>{
                new User {Email="sa@383.com",Password=Crypto.HashPassword("password"),ApiKey=CustomHelpers.GetApiKey(),Role=Roles.StoreAdmin},
            };
            
            users.ForEach(u => context.Users.AddOrUpdate(u));
            
        }
    }
}
