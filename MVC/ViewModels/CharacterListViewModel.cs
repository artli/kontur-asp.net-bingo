using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class CharacterListViewModel
    {
        public IEnumerable<CharacterViewModel> Characters { get; set; }
        public Cart Cart { get; set; }
        public bool UserHasSavedVotes { get; set; }
    }
}