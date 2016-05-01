using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Required]
        public int CommentID { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string Text { get; set; }
    }
}