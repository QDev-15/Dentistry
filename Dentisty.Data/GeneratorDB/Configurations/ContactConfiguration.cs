using Dentistry.Data.GeneratorDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Data.GeneratorDB.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.PhoneNumber).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Message).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.HasOne(x=> x.Branches).WithMany(x => x.Contacts).HasForeignKey(x => x.BranchesId).OnDelete(DeleteBehavior.SetNull).IsRequired(false);
            builder.HasOne(x => x.ProcessBy).WithMany(x => x.Missions).HasForeignKey(x => x.ProcessById).OnDelete(DeleteBehavior.SetNull).IsRequired(false);
        }
    }
}
