using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.GeneratorDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Data.GeneratorDB.Configurations
{
    public class HomeArticlesConfiguration : IEntityTypeConfiguration<HomeArticle>
    {
        public void Configure(EntityTypeBuilder<HomeArticle> builder)
        {
            builder.ToTable("HomeArticles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.BackgroundImage).WithMany(x => x.HomeArticles).HasForeignKey(x => x.BackgroundImageId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Article).WithMany(x => x.HomeArticles).HasForeignKey(x => x.ArticleId).OnDelete(DeleteBehavior.SetNull);
            

        }
    }
}
