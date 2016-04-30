using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    [Table("Votes")]
    public class Vote
    {
        [Required]
        public int VoteID { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public string Week { get; set; }
        [Required]
        public virtual ICollection<VoteItem> Items { get; set; }
    }
}