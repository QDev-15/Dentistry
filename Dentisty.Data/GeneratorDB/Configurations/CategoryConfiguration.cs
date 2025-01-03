using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Enums;
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
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Alias).IsRequired();
            builder.Property(x => x.Position).IsRequired().HasDefaultValue(CategoryPosition.None);
            builder.Property(x => x.Sort).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.IsParent).HasDefaultValue(false);
            builder.Property(x => x.ParentId).HasDefaultValue(null);
            builder.HasOne(x => x.Image).WithMany(x => x.Categories).HasForeignKey(x => x.ImageId).IsRequired(false);
            builder.HasMany(x => x.Articles).WithOne(x=>x.Category).HasForeignKey(x=>x.CategoryId);
            builder.HasOne(x => x.Parent).WithMany(x => x.Categories).HasForeignKey(x => x.ParentId).IsRequired(false);
            builder.HasOne(x => x.AppUser).WithMany(x => x.Categories).HasForeignKey(x => x.UserId).IsRequired(false);
            builder.Property(x => x.CreatedDate).IsRequired();
        }
    }
}
