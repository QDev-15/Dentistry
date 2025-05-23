﻿using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.GeneratorDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Data.GeneratorDB.Configurations
{
    public class ArticlesConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Articles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Alias).IsUnicode().IsRequired();
            builder.HasIndex(x => new { x.Title, x.Alias }).HasDatabaseName("ix_article_title_alias").IsUnique(false);
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Tags).IsRequired(false);
            builder.Property(x => x.CreatedById).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.HasOne(x => x.Category).WithMany(x => x.Articles).HasForeignKey(x => x.CategoryId).IsRequired(false).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Images).WithMany(x => x.Articles).UsingEntity<Dictionary<string, object>>(
                "ArticlesImage",
            j => j
                .HasOne<ImageFile>()
                .WithMany()
                .HasForeignKey("ImagesId")
                .OnDelete(DeleteBehavior.Cascade), // Allow cascade delete for images
            j => j
                .HasOne<Article>()
                .WithMany()
                .HasForeignKey("ArticlesId")
                .OnDelete(DeleteBehavior.Restrict));
        }
    }
}
