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
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteItem> VoteItems { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CharacterConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new VoteConfiguration());
            modelBuilder.Configurations.Add(new VoteItemConfiguration());
        }
    }
}