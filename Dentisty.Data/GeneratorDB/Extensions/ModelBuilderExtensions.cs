using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dentistry.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Dentisty.Data.Common;
using Dentisty.Data.Common.Enums;
using Dentisty.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppSetting>().HasData( new AppSetting()
            {
                Id = 1,
                Name = "Nhiên Dentistry",
            });
            var roleAdminId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            var roleUserId = new Guid("8D04DCE2-945A-435D-BBA4-DF3F325983DC");
            var userId = new Guid("69BD714F-9576-45CA-B5B7-F00649BE00DE");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleAdminId,
                Name = "admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role"
            },
            new AppRole
            {
                Id = roleUserId,
                Name = "Nick",
                NormalizedName = "NICK",
                Description = "User role"
            });

            var hasher = new PasswordHasher<AppUser>();

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "nguyenquynhvp.ictu@gmail.com",
                NormalizedEmail = "nguyenquynhvp.ictu@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@1234"),
                SecurityStamp = string.Empty,
                FirstName = "Quynh",
                LastName = "Nguyen",
                DisplayName = "Nguyễn Hữu Quỳnh",
                Dob = new DateTime(1990, 01, 31)
            },
            new AppUser
            {
                Id = userId,
                UserName = "User Default",
                NormalizedUserName = "Nick QN",
                Email = "quynhvpit@outlook.com",
                NormalizedEmail = "quynhvpit@outlook.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "User@1234"),
                SecurityStamp = string.Empty,
                FirstName = "Nick",
                LastName = "Qaury",
                DisplayName = "Nick Qaury Normal",
                Dob = new DateTime(1995, 01, 01),
                
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleAdminId,
                UserId = adminId
            }, new IdentityUserRole<Guid> { 
                RoleId = roleUserId,
                UserId = userId  
            }, new IdentityUserRole<Guid> { 
                RoleId = roleAdminId,
                UserId = userId
            });
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    ParentId = null,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Răng Sứ",
                    Alias = "rang-su",
                    Sort = 1,
                    UserId = adminId,
                },
                new Category()
                {
                    Id = 2,
                    ParentId = null,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Niềng Răng",
                    Alias = "nieng-rang",
                    Sort = 2,
                    UserId= adminId,   
                },
                new Category()
                {
                    Id = 3,
                    ParentId = null,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Bệnh lý",
                    Alias = "benh-ly",
                    Sort = 3,
                    UserId= adminId,   
                },
                new Category()
                {
                    Id = 4,
                    ParentId = null,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Giới thiệu",
                    Alias = "gioi-thieu",
                    Sort = 4,
                    UserId= adminId,   },
                new Category()
                {
                    Id = 5,
                    ParentId = null,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Liên hệ",
                    Alias = "lien-he",
                    Sort  = 5,
                    UserId= adminId,
                },
                 new Category()
                 {
                     Id = 6,
                     ParentId = 1,
                     IsActive = true,
                     CreatedDate = DateTime.Now,
                     Name = "Tiêu chí răng sứ",
                     Alias = "tieu-chi-rang-su",
                     Sort = 6,
                     UserId = adminId,
                     
                 }

            );
              
            modelBuilder.Entity<Article>().HasData(
                new Article()
                {
                    Id = 1,
                    Alias = "bai-viet-test",
                    Title = "Sự phát triển của răng sứ",
                    CreatedDate = DateTime.Now,
                    Description = "Bài viết test.",
                    CategoryId = 1,
                    CreatedById = adminId,
                }
            );
        }
    }
}