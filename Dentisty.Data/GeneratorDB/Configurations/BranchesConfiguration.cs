using Dentisty.Data.GeneratorDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dentisty.Data.GeneratorDB.Configurations
{
    public class BranchesConfiguration : IEntityTypeConfiguration<Branches>
    {
        public void Configure(EntityTypeBuilder<Branches> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Code).IsRequired(false).IsUnicode().HasMaxLength(10);
            builder.Property(x => x.PhoneNumber).IsRequired();
            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(true);
        }
    }
}
