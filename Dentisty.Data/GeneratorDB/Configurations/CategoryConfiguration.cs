using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Data.GeneratorDB.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Active).HasDefaultValue(true);
            builder.Property(x => x.ParentId).HasDefaultValue(null);
            builder.HasMany(x => x.Articles).WithOne(x=>x.Category).HasForeignKey(x=>x.CategoryId);
            builder.HasOne(x => x.Parent).WithMany(x => x.Categories).HasForeignKey(x => x.ParentId).IsRequired(false);
            builder.HasOne(x => x.AppUser).WithMany(x => x.Categories).HasForeignKey(x => x.UserId).IsRequired(false);
            builder.Property(x => x.CreatedDate).IsRequired();
        }
    }
}
