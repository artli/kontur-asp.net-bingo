using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string LoginName { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}