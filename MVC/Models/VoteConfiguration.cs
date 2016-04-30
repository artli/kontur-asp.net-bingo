using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MVC.Models
{
    public class VoteConfiguration : EntityTypeConfiguration<Vote>
    {
        public VoteConfiguration()
        {
            ToTable("Votes");
            Property(v => v.VoteID).IsRequired();
            Property(v => v.Week).IsRequired();
        }
    }
}