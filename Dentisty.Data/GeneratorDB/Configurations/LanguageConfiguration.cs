using Dentistry.Data.GeneratorDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Data.GeneratorDB.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Languages");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Code).IsRequired().IsUnicode(false).HasMaxLength(5);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(20);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.HasMany(x => x.Articles).WithOne(x => x.Language).HasForeignKey(x => x.LanguageId).IsRequired(false);
        }
    }
}
