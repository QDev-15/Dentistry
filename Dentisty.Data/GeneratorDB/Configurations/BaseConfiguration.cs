using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dentistry.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Configurations
{
    public class BaseConfiguration : IEntityTypeConfiguration<Base>
    {
        public void Configure(EntityTypeBuilder<Base> builder)
        {
            builder.ToTable("Bases");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(500).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(true);
        }
    }
}
