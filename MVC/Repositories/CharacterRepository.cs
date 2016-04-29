using MVC.Infrastructure;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Repositories
{
    public class CharacterRepository : RepositoryBase<Character>, ICharacterRepository
    {
        public CharacterRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }

    public interface ICharacterRepository : IRepository<Character>
    {

    }
}