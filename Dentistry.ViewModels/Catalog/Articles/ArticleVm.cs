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
        public string CoverImage
        {
            get
            {
                var value = "/assets/img/no-image.jpg";
                var img = Utilities.GetImageLink(Description);
                if (!string.IsNullOrEmpty(img) && img == value)
                {
                    if (Images.Any())
                    {
                        value = Images.FirstOrDefault().ThumbPath ?? Images.FirstOrDefault().Path;
                    }
                } else
                {
                    value = img;
                }
                return value;
            }
        }

    }
}
