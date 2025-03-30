
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dentistry.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Data.GeneratorDB.Configurations
{
    public class LoggerConfiguration : IEntityTypeConfiguration<Logger>
    {
        public void Configure(EntityTypeBuilder<Logger> builder)
        {
            builder.ToTable("Loggers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.HasIndex(x => x.Id).HasDatabaseName("ix_log_id").IsUnique(false);
            builder.Property(x => x.Body).IsRequired();
            builder.Property(x => x.IdAddress).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

        }
    }
}