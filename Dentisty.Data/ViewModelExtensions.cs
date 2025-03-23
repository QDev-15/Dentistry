using Dentistry.Common;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Branches;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Catalog.Logger;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data.GeneratorDB.Entities;
using Newtonsoft.Json;

namespace Dentisty.Data
{
    public static class ViewModelExtensions
    {
        private static string TimeZoneDefault = SystemConstants.TimeZoneDefaultId;
        public static LoggerVm ReturnViewModel(this Logger item)
        {
            if (item == null) { return new LoggerVm(); }

            var vm = new LoggerVm()
            {
                Id = item.Id,
                Body = item.Body,
                CreatedDate = item.CreatedDate,
                IdAddress = item.IdAddress,
                Title = item.Title,
                UserId = item.UserId
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
                TrackVisitors = item.TrackVisitors,
                Categories = item.Categories ?? "",
                Doctors = item.Doctors ?? "",
                Articles = item.Articles ?? "",
                News = item.News ?? "",
                Feedbacks = item.Feedbacks ?? "",
                Products = item.Products ?? "",
                CategoryProducts = item.CategoryProducts ?? "",
                BranchesTitle = item.BranchesTitle,
                CompanyAddress = item.CompanyAddress,
                CompanyEmail = item.CompanyEmail,
                CompanyName = item.CompanyName,
                CompanyPhone = item.CompanyPhone,
                CompanyTitle = item.CompanyTitle,
                CompanyWebsite = item.CompanyWebsite,
                Tiktok = item.Tiktok,
                CategoryListProductSubTitle = item.CategoryListProductSubTitle,
                CategoryListProductTitle = item.CategoryListProductTitle,
                CategoryListSubTitle = item.CategoryListSubTitle,
                CategoryListTitle = item.CategoryListTitle,
                DoctorListSubTitle = item.DoctorListSubTitle,
                DoctorListTitle = item.DoctorListTitle,
                NewsListTitle = item.NewsListTitle,
                FeedbackListTitle = item.FeedbackListTitle,
                ShowCategoryProductList = item.ShowCategoryProductList
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
                IdLogin = item.IdLogin ?? "",
                TimeZone = item.TimeZone,
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
        public static ImageVm ReturnViewModel(this Dentistry.Data.GeneratorDB.Entities.ImageFile item) {
            if (item == null) return null;
            var vm = new ImageVm()
            {
               Id = item.Id,
               FileName = item.FileName,
               FileSize = item.FileSize,
               ThumbPath = item.ThumbPath,
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
                Image = item.Image.ReturnViewModel(),
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
                Level = item.Level,
                UpdatedDate = item.UpdatedDate,
                CreatedDate = item.CreatedDate,
                Description = item.Description ?? "",
                Position = item.Position,
                Type = item.Type,
                IsParent = item.IsParent,
                UserId = item.UserId,
                Image = item.Image.ReturnViewModel(),
                Parent = item.Parent != null ? new CategoryVm() {
                    Id = item.Parent.Id,
                    Alias = item.Parent.Alias,
                    Name = item.Parent.Name,
                    Sort = item.Parent.Sort,
                    IsActive = item.Parent.IsActive,
                    ParentId = item.Parent.ParentId,
                    ImageId = item.Parent.ImageId,
                    UpdatedDate = item.Parent.UpdatedDate,
                    CreatedDate = item.Parent.CreatedDate,
                    Description = item.Parent.Description ?? "",
                    Position = item.Parent.Position,
                    Type = item.Parent.Type,
                    Level = item.Parent.Level,
                    IsParent = item.Parent.IsParent,
                    UserId = item.Parent.UserId,
                } : null,
                Articles = item.Articles != null && item.Articles.Any() ? item.Articles.Select(x => x.ReturnViewModel()).ToList() : new List<ArticleVm>(),
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
                Category = item.Category == null ? new CategoryVm() : new CategoryVm()
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name,
                    Alias = item.Category.Alias,
                    Image = item.Category.Image.ReturnViewModel(),
                    Description = item.Category.Description ?? "",
                    CreatedDate = item.Category.CreatedDate,
                    UpdatedDate = item.Category.UpdatedDate,
                    Parent = item.Category.Parent == null ? null : new CategoryVm()
                    {
                        Id = item.Category.Parent.Id,
                        Name = item.Category.Parent.Name,
                        Alias = item.Category.Parent.Alias,
                        Image = item.Category.Parent.Image.ReturnViewModel(),
                        Description = item.Category.Parent.Description ?? "",
                        CreatedDate = item.Category.Parent.CreatedDate,
                        UpdatedDate = item.Category.Parent.UpdatedDate,
                        Parent = item.Category.Parent.Parent == null ? null : new CategoryVm()
                        {
                            Id = item.Category.Parent.Parent.Id,
                            Name = item.Category.Parent.Parent.Name,
                            Alias = item.Category.Parent.Parent.Alias,
                            Image = item.Category.Parent.Parent.Image.ReturnViewModel(),
                            Description = item.Category.Parent.Parent.Description ?? "",
                            CreatedDate = item.Category.Parent.Parent.CreatedDate,
                            UpdatedDate = item.Category.Parent.Parent.UpdatedDate
                        }
                    }
                },
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
                Tags = item.Tags,
                UpdatedDate = item.UpdatedDate
            };

        }
    }
}
