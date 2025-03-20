using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dentistry.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Configurations;


public class ImageConfiguration : IEntityTypeConfiguration<ImageFile>
{
    public void Configure(EntityTypeBuilder<ImageFile> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).HasDatabaseName("ix_image_id").IsUnique(false);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.FileName).IsRequired();
        builder.Property(x => x.Path).IsRequired();
        builder.HasMany(x => x.Articles).WithMany(x => x.Images);
        builder.Property(x => x.CreatedDate).IsRequired();
    }
}
