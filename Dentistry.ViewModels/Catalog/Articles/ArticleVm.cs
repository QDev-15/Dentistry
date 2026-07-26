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
        public int? AvatarId { get; set; }
        public ImageVm Avatar { get; set; }
        public IFormFile? AvatarFile { get; set; }
        public string? AvatarUrl { get; set; }

        public CategoryVm Category { get; set; }
        public UserVm CreatedBy { set; get; }
        public List<IFormFile> ImageFiles { get; set; }
        public string TagsJson { get; set; }
        public string ImageIds { get; set; }
        public string DisplayType 
        {
            get
            {
                if (Type == ArticleType.News)
                {
                    return "Tin tức";
                }
                else if (Type == ArticleType.FeedBack)
                {
                    return "Phản hồi";
                }
                else if (Type == ArticleType.Products)
                {
                    return "Sản phẩm";
                }
                else if (Type == ArticleType.Article)
                {
                    return "Bài viết";
                }
                else
                {
                    return "Không xác định";
                }
            }
        }
        public string CoverImage
        {
            get
            {
                return Avatar?.Path ?? "/assets/img/no-image.jpg";
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
