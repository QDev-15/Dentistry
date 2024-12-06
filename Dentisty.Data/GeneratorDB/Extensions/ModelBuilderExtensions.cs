using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dentistry.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Dentisty.Data.Common;
using Dentisty.Data.Common.Enums;

namespace Dentistry.Data.GeneratorDB.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
               new AppConfig() { Id = 1, Key = "hotline-hcm", Value = "19009090" },
               new AppConfig() { Id = 2, Key = "facebook", Value = "nhien86" },
               new AppConfig() { Id = 3, Key = "hotline-hanoi", Value = "19001010" }
               );
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = 1, Code = "vi", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = 2, Code = "en", Name = "English", IsDefault = false });
            // any guid
            var roleAdminId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            var roleUserId = new Guid("8D04DCE2-945A-435D-BBA4-DF3F325983DC");
            var userId = new Guid("69BD714F-9576-45CA-B5B7-F00649BE00DE");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleAdminId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            },
            new AppRole
            {
                Id = roleUserId,
                Name = "Nick",
                NormalizedName = "User",
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
            });
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    ParentId = null,
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    Name = "Răng Sứ",
                    Alias = "rang-su",
                    UserId = adminId,
                },
                new Category()
                {
                    Id = 2,
                    ParentId = null,
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    Name = "Niềng Răng",
                    Alias = "nieng-rang",
                    UserId= adminId,   
                },
                new Category()
                {
                    Id = 3,
                    ParentId = null,
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    Name = "Bệnh lý",
                    Alias = "benh-ly",
                    UserId= adminId,   
                },
                new Category()
                {
                    Id = 4,
                    ParentId = null,
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    Name = "Giới thiệu",
                    Alias = "gioi-thieu",
                    UserId= adminId,   },
                new Category()
                {
                    Id = 5,
                    ParentId = null,
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    Name = "Liên hệ",
                    Alias = "lien-he",
                    UserId= adminId,
                },
                 new Category()
                 {
                     Id = 6,
                     ParentId = 1,
                     Status = Status.Active,
                     CreatedDate = DateTime.Now,
                     Name = "Tiêu chí răng sứ",
                     Alias = "tieu-chi-rang-su",
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