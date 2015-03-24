using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GameManager.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace GameManager.DAL
{
    public class GameManagerContext : DbContext
    {
        public GameManagerContext() : base("GameManagerContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}