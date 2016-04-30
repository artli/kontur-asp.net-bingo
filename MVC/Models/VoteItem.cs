using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class VoteItem
    {
        public int VoteItemID { get; set; }
        public virtual Vote Vote { get; set; }
        public virtual Character Character { get; set; }
        public int Position { get; set; }
    }
}