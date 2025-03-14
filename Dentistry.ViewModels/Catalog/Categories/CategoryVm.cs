using Dentistry.Common;
using Dentistry.ViewModels.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Dentistry.ViewModels.Catalog.Categories
{
    public class CategoryVm
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Alias { set; get; }
        public int Sort { set; get; }
        public int? ParentId { set; get; }
        public int? ImageId { set; get; }
        public Guid? UserId { set; get; }
        public bool IsActive { get; set; } = true;
        public bool IsParent { get; set; } = false;
        public CategoryPosition? Position { set; get; } = CategoryPosition.None;
        public CategoryType? Type { set; get; } = CategoryType.None;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Description { set; get; }
        public IFormFile ImageFile { set; get; }
        public ImageVm Image {  set; get; }
        public CategoryVm? Parent { get; set; }
        public List<CategoryVm> ChildCategories { get; set; } = new List<CategoryVm>();
        public string CoverImage
        {
            get
            {
                var value = "/assets/img/no-image.jpg";
                if (Image != null && Image.Id > 0)
                {
                    value = Image.Path;
                }
                else
                {
                    value = Utilities.GetImageLink(Description);
                }
                return value;
            }
        }
    }
}