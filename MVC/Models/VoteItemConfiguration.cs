using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MVC.Models
{
    public class VoteItemConfiguration : EntityTypeConfiguration<VoteItem>
    {
        public VoteItemConfiguration()
        {
            ToTable("VoteItems");
            Property(v => v.VoteItemID).IsRequired();
            Property(v => v.Position).IsRequired();
        }
    }
}