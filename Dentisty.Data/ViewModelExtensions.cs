using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.System.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data
{
    public static class ViewModelExtensions
    {
        public static UserVm ReturnViewModel(this AppUser item)
        {
            if (item == null) return null;
            var vm = new UserVm()
            {
                DisplayName = item.DisplayName,
                Dob = item.Dob,
                Email = item.Email,
                FirstName = item.FirstName,
                Id = item.Id.ToString(),
                LastName = item.LastName,
                PhoneNumber = item.PhoneNumber,
                UserName = item.UserName
            };
            return vm;
        }
        public static ImageVm ReturnViewModel(this Image item) {
            if (item == null) return null;
            var vm = new ImageVm()
            {
               Id = item.Id,
               FileName = item.FileName,
               FileSize = item.FileSize,
               Path = item.Path,
               Type = item.Type
            };
            return vm;
        }
        public static SlideVm ReturnViewModel(this Slide item)
        {
            if (item == null)
            {
                return new SlideVm();
            }
            var slideVm = new SlideVm()
            {
                Id = item.Id,
                Name = item.Name,
                SubName = item.SubName,
                Description = item.Description,
                Image = item.Image == null ? new ImageVm() : new ImageVm()
                {
                    Id = item.Image.Id,
                    FileName = item.Image.FileName,
                    FileSize = item.Image.FileSize,
                    Path = item.Image.Path,
                    Type = item.Image.Type
                },
                Url = item.Url ?? "",
                UserId = item.UserId,
                IsActive = item.IsActive,
                SortOrder = item.SortOrder,
                UpdatedDate = item.UpdatedDate,
                CreatedDate = item.CreatedDate
            };
            return slideVm;
        }
        public static CategoryVm ReturnViewModel(this Category item)
        {
            if ( item == null)
            {
                return null;
            }
            var categoryVm = new CategoryVm()
            {
                Id = item.Id,
                Alias = item.Alias,
                Name = item.Name,
                Sort = item.Sort,
                IsActive = item.IsActive,
                ParentId = item.ParentId,
                UpdatedDate = item.UpdatedDate,
                CreatedDate = item.CreatedDate,
                Decription = item.Description,
                UserId = item.UserId,
                Parent = item.Parent != null ? new CategoryVm() { Id = item.Parent.Id, Name = item.Parent.Name, Alias = item.Parent.Alias } : null,
                ChildCategories = item.Categories != null && item.Categories.Any() ? item.Categories.Select(c => c.ReturnViewModel()).ToList() : new List<CategoryVm>()
            };
            return categoryVm;
        }
        public static ArticleVm ReturnViewModel(this Article item)
        {
            if (item == null) return null;
            return new ArticleVm
            {
                Id = item.Id,
                Alias = item.Alias,
                Category = item.Category.ReturnViewModel(),
                CategoryId = item.CategoryId,
                CreatedBy = item.CreatedBy.ReturnViewModel(),
                CreatedDate = item.CreatedDate,
                CreatedById = item.CreatedById,
                Description = item.Description,
                Images = item.Images.Select(x => x.ReturnViewModel()).ToList(),
                IsActive = item.IsActive,
                Title = item.Title,
                UpdatedDate = item.UpdatedDate
            };

        }
    }
}
