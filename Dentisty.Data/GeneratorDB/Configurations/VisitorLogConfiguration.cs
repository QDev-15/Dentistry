
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dentistry.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Dentisty.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Configurations
{
    public class VisitorLogConfiguration : IEntityTypeConfiguration<VisitorLog>
    {
        public void Configure(EntityTypeBuilder<VisitorLog> builder)
        {
            builder.ToTable("VisitorLogs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn().IsRequired(true); ;
            builder.HasIndex(x => x.VisitTime).HasDatabaseName("ix_visitorlog_time");
            // Cùng lý do với ActiveUserConfiguration: VisitorTrackingMiddleware tra cứu theo
            // (VisitorId, IpAddress) trên mọi request, thiếu index gây quét toàn bảng.
            builder.HasIndex(x => new { x.VisitorId, x.IpAddress }).HasDatabaseName("ix_visitorlog_visitor_ip");

        }
    }
}