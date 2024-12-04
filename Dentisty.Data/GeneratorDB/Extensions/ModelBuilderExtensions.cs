using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.GeneratorDB.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Dentisty.Data.Common;

namespace Dentistry.Data.GeneratorDB.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
               new AppConfig() {Id = 1, Key = "hotline-hcm", Value = "19009090" },
               new AppConfig() {Id = 2, Key = "facebook", Value = "nhien86" },
               new AppConfig() {Id = 3, Key = "hotline-hanoi", Value = "19001010" }
               );
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = 1, Code = "vi", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = 2, Code = "en", Name = "English", IsDefault = false });
            // any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
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
                Dob = new DateTime(2020, 01, 31)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
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
                    Name = "Răng Nhựa",
                    Alias = "rang-nhua",
                    UserId= adminId,
                }
            );
                   
            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation() { Id = 1, CategoryId = 1, Name = "Áo nam", LanguageId = 1, SeoAlias = "ao-nam", SeoDescription = "Sản phẩm áo thời trang nam", SeoTitle = "Sản phẩm áo thời trang nam" },
                new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Men Shirt", LanguageId = 2, SeoAlias = "men-shirt", SeoDescription = "The shirt products for men", SeoTitle = "The shirt products for men" },
                new CategoryTranslation() { Id = 3, CategoryId = 2, Name = "Áo nữ", LanguageId = 1, SeoAlias = "ao-nu", SeoDescription = "Sản phẩm áo thời trang nữ", SeoTitle = "Sản phẩm áo thời trang women" },
                new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Women Shirt", LanguageId = 2, SeoAlias = "women-shirt", SeoDescription = "The shirt products for women", SeoTitle = "The shirt products for women" }
            );
            modelBuilder.Entity<Article>().HasData(
                new Article()
                {
                    Id = 1,
                    Alias = "",
                    CreatedDate = DateTime.Now,
                    Description = "Bài viết test",
                    CategoryId = 1,
                    CreatedById = adminId,
                }
            );

            modelBuilder.Entity<Slide>().HasData(
              new Slide() { Id = 1, UserId = adminId, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 1, Url = "#", Status = Status.Active },
              new Slide() { Id = 2, UserId = adminId, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 2, Url = "#", Status = Status.Active },
              new Slide() { Id = 3, UserId = adminId, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 3, Url = "#", Status = Status.Active },
              new Slide() { Id = 4, UserId = adminId, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 4, Url = "#", Status = Status.Active },
              new Slide() { Id = 5, UserId = adminId, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 5, Url = "#", Status = Status.Active },
              new Slide() { Id = 6, UserId = adminId, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 6, Url = "#", Status = Status.Active }
             );
        }
    }
}