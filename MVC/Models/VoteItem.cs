using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    [Table("VoteItems")]
    public class VoteItem
    {
        [Required]
        public int VoteItemID { get; set; }
        [Required]
        public Vote Vote { get; set; }
        [Required]
        public Character Character { get; set; }
        [Required]
        public int Position { get; set; }
    }
}