
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dentistry.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Dentisty.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Configurations
{
    public class ActiveUserConfiguration : IEntityTypeConfiguration<ActiveUser>
    {
        public void Configure(EntityTypeBuilder<ActiveUser> builder)
        {
            builder.ToTable("ActiveUsers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn().IsRequired(true);
            builder.HasIndex(x => x.LastActive).HasDatabaseName("ix_activeUser_time");

        }
    }
}