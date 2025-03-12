using Dentistry.Data.Common.Constants;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Branches;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Catalog.Slide;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Services
{
    public class ApplicationService
    {
        private readonly ICategoryReposiroty _cat;
        private readonly IAppSettingRepository _setting;
        private readonly ICacheService _cache;
        private readonly IBranchesRepository _branchesRepository;
        private readonly ISlideRepository _slide;
        private readonly IDoctorRepository _doctor;
        private readonly IArticleRepository _article;
        public ApplicationService(IArticleRepository article, IDoctorRepository doctor,
            ISlideRepository slideRepository, IBranchesRepository branchesRepository,
            ICategoryReposiroty cat, IAppSettingRepository setting, ICacheService cache)
        {
            _cat = cat;
            _setting = setting;
            _cache = cache; 
            _branchesRepository = branchesRepository;
            _slide = slideRepository;
            _doctor = doctor;
            _article = article;
        }

        public async Task<ApplicationVm> GetApplication()
        {
            ApplicationVm application = new ApplicationVm();
            application.Settings = await GetAppSetting();
            application.Branches = await GetBranches();
            application.Categories = await GetCategories();
            return application;
        }
        public async Task<AppSettingVm> GetAppSetting()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys.AppSetting, async () =>
            {
                InvalidateCache(SystemConstants.CacheKeys.AppSettingDoctor);
                InvalidateCache(SystemConstants.CacheKeys.AppSettingCategory);
                InvalidateCache(SystemConstants.CacheKeys.AppSettingArticle);
                InvalidateCache(SystemConstants.CacheKeys.AppSettingNews);
                InvalidateCache(SystemConstants.CacheKeys.AppSettingProduct);
                InvalidateCache(SystemConstants.CacheKeys.AppSettingFeedback);
                return await _setting.GetById(1);
            });
        }
        public async Task<List<CategoryVm>> GetCategories()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys.AppCategory, async () =>
            {
                var categories = await _cat.GetParents();
                var categoryList = categories.Select(x => x.ReturnViewModel()).ToList();
                return categoryList;
            });
        }
        public async Task<List<BranchesVm>> GetBranches()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys.AppBranches, async () =>
            {
                var branches = await _branchesRepository.GetActive();
                return branches.ToList();
            });
        }
        public async Task<List<SlideVm>> GetMainSlides()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys.AppSlide, async () =>
            {
                var slides = await _slide.GetActiveSlides();
                return slides;
            });
        }
        
        
        public async Task<List<DoctorVm>> GetDoctorSlides()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys. AppSettingDoctor, async () =>
            {
                var appSetting = await GetAppSetting();
                var doctors = await _doctor.GetDoctorByIds(appSetting.Doctors);
                return doctors.ToList();
            });
        }
        public async Task<List<CategoryVm>> GetCategorySlides()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys.AppSettingCategory, async () =>
            {
                var appSetting = await GetAppSetting();
                var categories = await GetCategories();
                var listId = appSetting.Categories != null ? appSetting.Categories.Split(",") : [];
                var homeCats = new List<CategoryVm>();
                if (listId.Length > 0)
                {
                    foreach (var item in categories)
                    {
                        if (listId.Contains(item.Id.ToString()))
                        {
                            homeCats.Add(item);
                        }
                        foreach (var itemChild in item.ChildCategories)
                        {
                            if (listId.Contains(itemChild.Id.ToString()))
                            {
                                homeCats.Add(itemChild);
                            }
                        }
                    }
                    homeCats = homeCats.OrderBy(c => c.IsParent).Take(4).ToList();
                }
                return homeCats;
            });
        }
        public async Task<List<ArticleVm>> GetArticleSlides()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys.AppSettingArticle, async () =>
            {
                var appSetting = await GetAppSetting();
                var articles = await _article.GetArticleByIds(appSetting.Articles);
                return articles.ToList();
            });
        }
        public async Task<List<ArticleVm>> GetArticlesNews()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys.AppSettingNews, async () =>
            {
                var appSetting = await GetAppSetting();
                var articles = await _article.GetArticleByIds(appSetting.News);
                return articles.ToList();
            });
        }
        public async Task<List<ArticleVm>> GetArticlesProduct()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys.AppSettingProduct, async () =>
            {
                var appSetting = await GetAppSetting();
                var articles = await _article.GetArticleByIds(appSetting.Products);
                return articles.ToList();
            });
        }      
        public async Task<List<ArticleVm>> GetArticlesFeddback()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys.AppSettingFeedback, async () =>
            {
                var appSetting = await GetAppSetting();
                var articles = await _article.GetArticleByIds(appSetting.Feedbacks);
                return articles.ToList();
            });
        }
        public async Task<List<ArticleVm>> GetArticlesHotNews()
        {
            return await _cache.GetOrSetAsync(SystemConstants.CacheKeys.ArticleChange, async () =>
            {
                var articles = await _article.GetArticleNew();
                return articles.ToList();
            });
        }
        
        
        public void InvalidateCache(string key)
        {
            _cache.RemoveAsync(key); // Xóa cache khi có cập nhật từ DB
        }
    }
}
