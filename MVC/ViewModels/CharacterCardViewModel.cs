using MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class CharacterCardViewModel
    {
        public CharacterViewModel CharacterViewModel { get; set; }
        public bool UserHasSavedVotes;
    }
}