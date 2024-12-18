using Dentisty.Data.GeneratorDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.GeneratorDB.Configurations
{
    internal class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x =>x.Name).IsRequired();
            builder.Property(x => x.Dob).IsRequired();
            builder.Property(x => x.Position).IsRequired();
            builder.Property(x => x.Description).IsRequired(false);
            builder.HasOne(x => x.Avatar).WithMany(x => x.Doctors).HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
        }

    }
}
