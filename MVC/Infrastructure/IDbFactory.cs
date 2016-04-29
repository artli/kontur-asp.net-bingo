using MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BingoDbContext Init();
    }
}