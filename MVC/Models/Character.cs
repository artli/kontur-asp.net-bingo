using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Character
    {
        public int CharacterID { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ImageName { get; set; }
    }
}