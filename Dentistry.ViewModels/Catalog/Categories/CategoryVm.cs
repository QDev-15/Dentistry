using Dentistry.Common;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        public CategoryLevel Level { set; get; } = CategoryLevel.Level1;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Description { set; get; }
        public IFormFile ImageFile { set; get; }
        public ImageVm Image {  set; get; }
        public CategoryVm? Parent { get; set; }
        public List<CategoryVm> ChildCategories { get; set; } = new List<CategoryVm>();
        public List<ArticleVm> Articles { get; set; } = new List<ArticleVm>();
        public string CoverImage
        {
            get
            {
                var value = "/assets/img/no-image.jpg";
                if (Image != null && Image.Id > 0)
                {
                    value = Image.Path;
                    //value = string.IsNullOrEmpty(Image.ThumbPath) ? Image.Path : Image.ThumbPath;
                }
                else
                {
                    value = Utilities.GetImageLink(Description);
                }
                return value;
            }
        }

        public string AliasType
        {
            get {
                
                if (Type != CategoryType.None)
                {
                    return Type.GetAliasDisplayName();
                } else if (Parent != null && Parent.Type != CategoryType.None)
                {
                    return Parent.Type.GetAliasDisplayName();
                } else if (Parent != null && Parent.Parent != null && Parent.Parent.Type != CategoryType.None)
                {
                    return Parent.Parent.Type.GetAliasDisplayName();
                } else
                {

                }
                return "none";
            }
        }

        public string SubName
        {
            get
            {
                if (Level == CategoryLevel.Level1)
                {
                    return "(Vị trí: " + Position.GetDisplayName() + ", Thứ tự: " + Sort.ToString() + ", Loại: " + Type.GetDisplayName() + ")";
                }
                else
                {
                    return "(Loại Danh mục: " + Type.GetDisplayName() + ")";
                }
            }
        }
        public string PositionName
        {
            get
            {
                return Position.GetDisplayName();
            }
        }
        public string TypeName
        {
            get
            {
                return Type.GetDisplayName();
            }
        }
        public int LevelValue
        {
            get
            {
                return (int)Level;
            }
        }
        public string LevelName
        {
            get
            {
                return Level.ToString();
            }
        }
    }
}