using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class CharacterCardViewModel
    {
        public CharacterWithState CharacterWithState { get; set; }
        public bool UserHasSavedVotes;
    }
}