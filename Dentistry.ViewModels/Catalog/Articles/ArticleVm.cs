using Dentistry.Common;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Enums;
using Dentistry.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;


namespace Dentistry.ViewModels.Catalog.Articles
{
    public class ArticleVm
    {
        public int Id { get; set; }
        public string Alias { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ArticleType Type { get; set; }
        public bool IsActive { get; set; }
        public bool IsDraft { get; set; } = false;
        public Guid CreatedById { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Tags { get; set; }
        public List<ImageVm> Images { get; set; } = new List<ImageVm>();

        public CategoryVm Category { get; set; }
        public UserVm CreatedBy { set; get; }
        public List<IFormFile> ImageFiles { get; set; }
        public string TagsJson { get; set; }
        public string ImageIds { get; set; }
        public string CoverImage
        {
            get
            {
                var value = "/assets/img/no-image.jpg";
                if (Images.Any())
                {
                    value = Images.FirstOrDefault().Path;
                    //value = Images.FirstOrDefault().ThumbPath??Images.FirstOrDefault().Path;
                }
                else
                {
                    value = Utilities.GetImageLink(Description);
                }
                return value;
            }
        }
        public string CategoryName
        {
            get
            {
                if (Category != null)
                {
                    return Category.Name;
                }
                else
                {
                    return "Không xác định";
                }
            }
        }      
        public string CreatedByName
        {
            get
            {
                if (CreatedBy != null)
                {
                    return CreatedBy.DisplayName;
                }
                else
                {
                    return "Không xác định";
                }
            }
        }

    }
}
