using MVC.Infrastructure;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Repositories
{
    public class VoteRepository : RepositoryBase<Vote>, IVoteRepository
    {
        public VoteRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }

    public interface IVoteRepository : IRepository<Vote>
    {

    }
}