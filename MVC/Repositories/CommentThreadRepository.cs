using MVC.Infrastructure;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MVC.Repositories
{
    public class CommentThreadRepository : RepositoryBase<CommentThread>, ICommentThreadRepository
    {
        public CommentThreadRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public override void Add(CommentThread entity)
        {
            dbSet.Add(entity);
        }

        //public override void Update(CommentThread entity)
        //{
        //    dbSet.Attach(entity);
        //    dataContext.Entry(entity).State = EntityState.Modified;
        //}

        public override void Delete(CommentThread entity)
        {
            dbSet.Remove(entity);
        }

        public override void Delete(Expression<Func<CommentThread, bool>> where)
        {
            IEnumerable<CommentThread> objects = dbSet.Where<CommentThread>(where).AsEnumerable();
            foreach (CommentThread obj in objects)
                dbSet.Remove(obj);
        }

        public override CommentThread GetById(int id)
        {
            return dbSet.Find(id);
        }

        public override IEnumerable<CommentThread> GetAll()
        {
            return dbSet.Include("Comments").ToList();
        }

        public override IEnumerable<CommentThread> GetMany(Expression<Func<CommentThread, bool>> where)
        {
            return dbSet.Include("Comments").Where(where).ToList();
        }

        public override CommentThread Get(Expression<Func<CommentThread, bool>> where)
        {
            return dbSet.Include("Comments").Include("Comments.User").Where(where).FirstOrDefault<CommentThread>();
        }
    }

    public interface ICommentThreadRepository : IRepository<CommentThread>
    {

    }
}