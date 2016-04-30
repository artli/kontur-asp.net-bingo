using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Vote
    {
        public int VoteID { get; set; }
        // public User User { get; set; }
        public string Week { get; set; }
        public virtual ICollection<VoteItem> Items { get; set; }
    }
}