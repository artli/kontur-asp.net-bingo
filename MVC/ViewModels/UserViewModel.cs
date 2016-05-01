using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public String Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [DisplayName("Remember me")]
        public bool IsPersistent { get; set; }
    }
}