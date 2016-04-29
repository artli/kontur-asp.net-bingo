using MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Infrastructure
{
    public class DbFactory : IDbFactory
    {
        BingoDbContext dbContext;

        public BingoDbContext Init()
        {
            return dbContext ?? (dbContext = new BingoDbContext());
        }

        public void Dispose()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}