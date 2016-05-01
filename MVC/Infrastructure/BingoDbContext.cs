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
        public BingoDbContext() : base("GoTBingoDatabase") { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteItem> VoteItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentThread> CommentThreads { get; set; }
        //public List<Comment> Comments { get; set; } = new List<Comment>();
        //public List<CommentThread> CommentThreads { get; set; } = new List<CommentThread>();

        public virtual void Commit()
        {
            try
            {
                base.SaveChanges();
            } catch (Exception e)
            {
                throw;
            }
        }
    }
}