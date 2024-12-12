using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Slide;
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
                return new CategoryVm();
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
                UserId = item.UserId,
                Parent = item.Parent != null ? new CategoryVm
                {
                    Id = item.Parent.Id,
                    Alias = item.Parent.Alias,
                    Name = item.Parent.Name,
                    Sort = item.Parent.Sort,
                    IsActive = item.Parent.IsActive,
                    UpdatedDate = item.Parent.UpdatedDate,
                    CreatedDate = item.Parent.CreatedDate,
                    UserId = item.Parent.UserId,
                } : null,
                ChildCategories = item.Categories != null && item.Categories.Any() ? item.Categories.Select(c => new CategoryVm {
                    Id = c.Id,
                    Alias = c.Alias,
                    Sort = c.Sort,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    UpdatedDate = c.UpdatedDate,
                    CreatedDate = c.CreatedDate,
                    UserId = c.UserId,
                }).ToList() : new List<CategoryVm>()
            };
            return categoryVm;
        }
    }
}
