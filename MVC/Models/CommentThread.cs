using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    [Table("CommentThreads")]
    public class CommentThread
    {
        [Required]
        public int CommentThreadID { get; set; }
        [Required]
        [Index(IsUnique = true)]
        public Character Character { get; set; }
        [Required]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}