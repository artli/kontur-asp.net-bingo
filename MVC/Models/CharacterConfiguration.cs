using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MVC.Models
{
    public class CharacterConfiguration : EntityTypeConfiguration<Character>
    {
        public CharacterConfiguration()
        {
            ToTable("Characters");
            Property(g => g.CharacterID).IsRequired();
            Property(g => g.Name).IsRequired();
            Property(g => g.Gender).IsRequired();
            Property(g => g.Description).IsRequired();
            Property(g => g.Price).IsRequired();
        }
    }
}