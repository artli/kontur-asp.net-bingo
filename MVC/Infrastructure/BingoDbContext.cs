using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MVC.Models;

namespace MVC.Repositories
{
    public class BingoDbContext : DbContext
    {
        public BingoDbContext() : base("GoTBigoDatabase") { }

        public DbSet<Character> Characters { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CharacterConfiguration());
        }
    }
}