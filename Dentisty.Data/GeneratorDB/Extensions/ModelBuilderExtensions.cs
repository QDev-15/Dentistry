using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Enums;
using Dentisty.Data.GeneratorDB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                CategoryListTitle = "Dịch Vụ Thẩm Mỹ",
                CategoryListSubTitle = "Nhiên tập trung cung cấp các dịch vụ nha khoa thẩm mỹ trong môi trường nhẹ nhàng, chu đáo, chuyên nghiệp. Đảm bảo mọi khách hàng tới Win Smile đều có những trải nhiệm nha khoa đẳng cấp, ưng ý nhất cho đường cười của chính mình.",
                CategoryListProductTitle = "Sản Phẩm Thẩm Mỹ",
                CategoryListProductSubTitle = "Nhiên tập trung cung cấp các dịch vụ nha khoa thẩm mỹ trong môi trường nhẹ nhàng, chu đáo, chuyên nghiệp. Đảm bảo mọi khách hàng tới Win Smile đều có những trải nhiệm nha khoa đẳng cấp, ưng ý nhất cho đường cười của chính mình."
            });
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            var roleAdminId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var roleUserId = new Guid("8D04DCE2-945A-435D-BBA4-DF3F325983DC");
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
                Name = "user",
                NormalizedName = "USER",
                Description = "User role"
            });

            var hasher = new PasswordHasher<AppUser>();

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "nhienadmin@gmail.com",
                NormalizedEmail = "nhienadmin@gmail.com",
                EmailConfirmed = true,
                IsActive = true,
                PasswordHash = hasher.HashPassword(null, "Admin@1234"),
                SecurityStamp = string.Empty,
                FirstName = "Nhiên",
                LastName = "Admin",
                DisplayName = "Nhiên",
                Dob = new DateTime(1990, 01, 31)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleAdminId,
                UserId = adminId
            });
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    ParentId = null,
                    IsParent = true,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Dịch vụ",
                    Alias = "dich-vu",
                    Sort = 1,
                    Position = CategoryPosition.MenuLef,
                    Type = CategoryType.Service,
                    UserId = adminId,
                },
                new Category()
                {
                    Id = 2,
                    ParentId = null,
                    IsParent = true,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Sản phẩm",
                    Alias = "san-pham",
                    Sort = 2,
                    Position = CategoryPosition.MenuLef,
                    Type = CategoryType.Products,
                    UserId= adminId,   
                },
                new Category()
                {
                    Id = 3,
                    ParentId = null,
                    IsParent = true,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Kiến thức",
                    Alias = "kien-thuc",
                    Sort = 3,
                    Position = CategoryPosition.MenuLef,
                    Type = CategoryType.advise,
                    UserId= adminId,   
                },
                new Category()
                {
                    Id = 4,
                    ParentId = null,
                    IsParent = true,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Tin tức",
                    Alias = "tin-tuc",
                    Sort = 4,
                    Position = CategoryPosition.MenuRight,
                    Type  = CategoryType.News,
                    UserId= adminId,   },
                new Category()
                {
                    Id = 5,
                    ParentId = null,
                    IsParent = true,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Tư vấn",
                    Alias = "tu-van",
                    Sort = 5,
                    Position = CategoryPosition.MenuRight,
                    Type = CategoryType.advise,
                    UserId = adminId,
                },
                new Category()
                {
                    Id = 6,
                    ParentId = null,
                    IsParent = true,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = "Liên hệ",
                    Alias = "lien-he",
                    Position = CategoryPosition.MenuRight,
                    Type = CategoryType.Support,
                    Sort  = 6,
                    UserId= adminId,
                },
                 new Category()
                 {
                     Id = 7,
                     ParentId = null,
                     IsParent = true,
                     IsActive = true,
                     CreatedDate = DateTime.Now,
                     Name = "Giới thiệu",
                     Alias = "gioi-thieu",
                     Position = CategoryPosition.MenuRight,
                     Type = CategoryType.About,
                     Sort = 7,
                     UserId = adminId,
                     
                 }
            );
        }
    }
}