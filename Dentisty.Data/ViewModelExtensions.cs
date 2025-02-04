using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Branches;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.Catalog.Tags;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data.GeneratorDB.Entities;

namespace Dentisty.Data
{
    public static class ViewModelExtensions
    {
        public static TagsVm ReturnViewModel(this Tags item)
        {
            if (item == null) return new TagsVm();
            var vm = new TagsVm()
            {
              Id = item.Id,
              Name = item.Name,
              CreatedAt = item.CreatedAt,
              UpdatedAt = item.UpdatedAt
            };
            return vm;
        }
        public static BranchesVm ReturnViewModel(this Branches branches)
        {
            if (branches == null)
            {
                return new BranchesVm();
            }
            return new BranchesVm()
            {
                Id = branches.Id,
                Name = branches.Name,
                Address = branches.Address,
                Code = branches.Code,
                IsActive = branches.IsActive,
                CreatedAt = branches.CreatedAt,
                PhoneNumber = branches.PhoneNumber,
                UpdatedAt = branches.UpdatedAt
            };
        }
        public static AppSettingVm ReturnViewModel(this AppSetting item)
        {
            if (item == null)
            {
                return null;
            }
            var model = new AppSettingVm()
            {
                Id = item.Id,
                Name = item.Name,
                EndWork = item.EndWork,
                StartWork = item.StartWork,
                HotlineHaNoi = item.HotlineHaNoi,
                Facebook = item.Facebook,
                Instagram = item.Instagram,
                Twitter = item.Twitter,
                Youtube = item.Youtube,
                ZaloHotline = item.ZaloHotline,
                ShowCategoryList = item.ShowCategoryList,
                ShowArtileSlideList = item.ShowArtileSlideList,
                ShowDoctorSlideList = item.ShowDoctorSlideList,
                ShowContactList = item.ShowContactList,
                ShowFeedbackList = item.ShowFeedbackList,
                ShowNewsList = item.ShowNewsList,
                ShowProductList = item.ShowProductList,
                ShowToolBarTop = item.ShowToolBarTop,
                Categories = item.Categories ?? "",
                Doctors = item.Doctors ?? "",
                Articles = item.Articles ?? "",
                News = item.News ?? "",
                Feedbacks = item.Feedbacks ?? "",
                Products = item.Products ?? ""
            };
            return model;
        }
        public static UserVm ReturnViewModel(this AppUser item)
        {
            if (item == null)
            {
                return null;
            }
            var vm = new UserVm()
            {
                DisplayName = item.DisplayName,
                Dob = item.Dob,
                Email = item.Email,
                FirstName = item.FirstName,
                Id = item.Id.ToString(),
                LastName = item.LastName,
                PhoneNumber = item.PhoneNumber,
                UserName = item.UserName,
                Avatar = item.Avatar.ReturnViewModel()
            };
            return vm;
        }
        public static DoctorVm ReturnViewModel(this Doctor item)
        {
            if (item == null)
            { 
                return null;
            }
            var vm = new DoctorVm()
            {
                Id = item.Id,
                Description = item.Description,
                Avatar = item.Avatar.ReturnViewModel(),
                Dob = item.Dob,
                Name = item.Name,
                Alias = item.Alias,
                Position = item.Position,
                PositionExtent = item.PositionExtent,
                Profile = item.Profile
            };
            return vm;
        }
        public static ContactVm ReturnViewModel(this Contact item)
        {
            if (item == null)
            {
                return null;
            }
            var vm = new ContactVm()
            {
                CreatedDate = item.CreatedDate,
                Email = item.Email ?? "",
                Id = item.Id,
                IsActive = item.IsActive,
                Message = item.Message,
                TimeBook = item.TimeBook,
                Branch = item.Branches.ReturnViewModel(),
                Note = item.Note ?? "",
                Name = item.Name,
                PhoneNumber = item.PhoneNumber,
                UpdatedDate = item.UpdatedDate,
                ProcessBy = item.ProcessBy.ReturnViewModel()
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
                Caption = item.Caption ?? "",
                SubName = item.SubName ?? "",
                Description = item.Description ?? "",
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
                ImageId = item.ImageId,
                UpdatedDate = item.UpdatedDate,
                CreatedDate = item.CreatedDate,
                Decription = item.Description ?? "",
                Position = item.Position,
                Type = item.Type,
                IsParent = item.IsParent,
                UserId = item.UserId,
                Image = item.Image.ReturnViewModel(),
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
                IsDraft = item.IsDraft,
                Title = item.Title,
                Type = item.Type,
                UpdatedDate = item.UpdatedDate
            };

        }
    }
}
