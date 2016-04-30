using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MVC.Models
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");
            Property(u => u.UserID).IsRequired();
            Property(u => u.LoginName).IsRequired();
        }
    }
}