using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVC.Models;

namespace MVC.ViewModels
{
    public class UserViewModel
    {

        public UserViewModel(User user)
        {
            if (user == null)return;
            Login = user.LoginName;
        }

        public UserViewModel()
        {
            
        }
        [Required]
        public String Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [DisplayName("Remember me")]
        public bool IsPersistent { get; set; }
    }
}