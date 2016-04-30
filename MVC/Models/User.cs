using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    [Table("Users")]
    public class User
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string LoginName { get; set; }
        [Required]
        public Byte[] PwdHash { get; set; }
        [Required]
        public ICollection<Vote> Votes { get; set; }

        public UserRole Role { get; set; }
    }
}