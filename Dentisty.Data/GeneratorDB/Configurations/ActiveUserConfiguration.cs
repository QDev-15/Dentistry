
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dentistry.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Dentisty.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Configurations
{
    public class ActiveUserConfiguration : IEntityTypeConfiguration<ActiveUser>
    {
        public void Configure(EntityTypeBuilder<ActiveUser> builder)
        {
            builder.ToTable("ActiveUsers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn().IsRequired(true);
            builder.HasIndex(x => x.LastActive).HasDatabaseName("ix_activeUser_time");
            // VisitorTrackingMiddleware tra cứu theo (VisitorId, IpAddress) trên MỌI request -
            // thiếu index này khiến câu SELECT phải quét toàn bảng, càng chậm dần khi bảng phình
            // to, và dễ bị khoá/timeout khi trùng thời điểm với ActiveUserCleanupService đang xoá
            // dữ liệu cũ (đã tận mắt thấy request bị timeout 30s+ vì lý do này).
            builder.HasIndex(x => new { x.VisitorId, x.IpAddress }).HasDatabaseName("ix_activeUser_visitor_ip");

        }
    }
}