using MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<CharacterViewModel> Characters { get; set; }
        public bool UserHasSavedVotes { get; set; }
    }
}