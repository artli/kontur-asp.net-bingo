using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public enum Gender
    {
        [Description("Мужчина")]
        Male,
        [Description("Женщина")]
        Female
    }
}