using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class CharacterViewModel
    {
        public Character Character { get; set; }
        public CharacterState State { get; set; }
        public CommentThread CommentThread { get; set; }
    }
}