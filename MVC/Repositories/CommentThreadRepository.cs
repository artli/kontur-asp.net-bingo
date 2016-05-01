using MVC.Infrastructure;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Repositories
{
    public class CommentThreadRepository : RepositoryBase<CommentThread>, ICommentThreadRepository
    {
        public CommentThreadRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }

    public interface ICommentThreadRepository : IRepository<CommentThread>
    {

    }
}