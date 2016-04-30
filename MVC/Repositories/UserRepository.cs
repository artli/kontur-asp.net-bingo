using MVC.Infrastructure;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }

    public interface IUserRepository : IRepository<User>
    {

    }
}