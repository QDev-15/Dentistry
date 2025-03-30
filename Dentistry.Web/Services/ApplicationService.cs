using Dentistry.Common;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Branches;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.Enums;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services.Interfaces;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace Dentisty.Web.Services
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
        private readonly IWebHostEnvironment _env;

        public ApplicationService(IArticleRepository article, IDoctorRepository doctor,
            ISlideRepository slideRepository, IBranchesRepository branchesRepository, IWebHostEnvironment env,
            ICategoryReposiroty cat, IAppSettingRepository setting, ICacheService cache)
        {
            _env = env;
            _cat = cat;
            _setting = setting;
            _cache = cache; 
            _branchesRepository = branchesRepository;
            _slide = slideRepository;
            _doctor = doctor;
            _article = article;
        }
        // app setting
        public async Task<AppSettingVm> GetAppSetting()
        {
            //if (_env.IsDevelopment())
            //{
            //    return await _setting.GetFirst();
            //}
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Setting, async () =>
            {
                return await _setting.GetFirst();
            });
        }
        
        public async Task<List<BranchesVm>> GetBranches()
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Branches + "GetBranches", async () =>
            {
                var branches = await _branchesRepository.GetActive();
                return branches.ToList();
            });
        }
        public async Task<List<SlideVm>> GetMainSlides()
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Slide + "GetMainSlides", async () =>
            {
                var slides = await _slide.GetActiveSlides();
                return slides;
            });
        }
        public async Task<List<DoctorVm>> GetDoctorSlides()
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Doctor + "GetDoctorSlides", async () =>
            {
                var appSetting = await GetAppSetting();
                var doctors = await _doctor.GetDoctorByIds(appSetting.Doctors);
                return doctors.ToList();
            });
        }
        // Category
        public async Task<List<CategoryVm>> GetAllCategories()
        {
            //if (_env.IsDevelopment())
            //{
            //    var categories = await _cat.GetParents();
            //    var categoryList = categories.Select(x => x.ReturnViewModel()).ToList();
            //    return categoryList;
            //}
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Category + "GetAllCategories", async () =>
            {
                var categories = await _cat.GetParents();
                var categoryList = categories.Select(x => x.ReturnViewModel()).ToList();
                return categoryList;
            });
        }
        public async Task<CategoryVm> GetCategoryByAlias(string alias)
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Category + "GetCategoryByAlias" + alias, async () =>
            {
                var categories = await GetAllCategories();
                if (!categories.Any())
                {
                    return new CategoryVm();
                }
                foreach (var item in categories)
                { // lv1
                    if (item.Alias.ToLower() == alias.ToLower())
                    {
                        return item;
                    }
                    else if (item.ChildCategories.Any())
                    {
                        foreach (var itemlv2 in item.ChildCategories)
                        { // lv2
                            if (itemlv2.Alias.ToLower() == alias.ToLower())
                            {
                                return itemlv2;
                            }
                            else if (itemlv2.ChildCategories.Any())
                            {
                                foreach (var itemlv3 in itemlv2.ChildCategories)
                                { // lv3
                                    if (itemlv3.Alias.ToLower() == alias.ToLower())
                                    {
                                        return itemlv3;
                                    }
                                }
                            }
                        }
                    }
                }
                return new CategoryVm();
            });
        }
        public async Task<CategoryVm> GetCategoryById(int id)
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Category + "GetCategoryById" + id, async () =>
            {
                var category = new CategoryVm();
                var categories = await GetAllCategories();
                if (!categories.Any())
                {
                    return new CategoryVm();
                }
                foreach (var item in categories)
                { // lv1
                    if (item.Id == id)
                    {
                        return item;
                    }
                    else if (item.ChildCategories.Any())
                    {
                        foreach (var itemlv2 in item.ChildCategories)
                        { // lv2
                            if (itemlv2.Id == id)
                            {
                                return itemlv2;
                            }
                            else if (itemlv2.ChildCategories.Any())
                            {
                                foreach (var itemlv3 in itemlv2.ChildCategories)
                                { // lv3
                                    if (itemlv3.Id == id)
                                    {
                                        return itemlv3;
                                    }
                                }
                            }
                        }
                    }
                }
                return new CategoryVm();
            });
        }
        public async Task<List<CategoryVm>> GetCategoryServices()
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Category + "GetCategoryServices", async () =>
            {
                var appSetting = await GetAppSetting();
                var categories = await GetAllCategories();
                var listId = appSetting.Categories != null ? appSetting.Categories.Split(",") : [];
                var homeCats = new List<CategoryVm>();
                if (listId.Length > 0)
                {
                    foreach (var item in categories)
                    {
                        if (listId.Contains(item.Id.ToString()) && item.Type == CategoryType.Service)
                        {
                            homeCats.Add(item);
                        }
                        foreach (var itemChild in item.ChildCategories)
                        {
                            if (listId.Contains(itemChild.Id.ToString()) && itemChild.Type == CategoryType.Service)
                            {
                                homeCats.Add(itemChild);
                            }
                            foreach (var itemLv3 in itemChild.ChildCategories)
                            {
                                if (listId.Contains(itemLv3.Id.ToString()) && item.Type == CategoryType.Service)
                                {
                                    homeCats.Add(itemLv3);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in categories)
                    {
                        if (item.Type == CategoryType.Service)
                        {
                            homeCats.Add(item);
                        }
                        foreach (var itemChild in item.ChildCategories)
                        {
                            if (itemChild.Type == CategoryType.Service)
                            {
                                homeCats.Add(itemChild);
                            }
                            foreach (var itemChildLv3 in itemChild.ChildCategories)
                            {
                                if (itemChildLv3.Type == CategoryType.Service)
                                {
                                    homeCats.Add(itemChildLv3);
                                }
                            }
                        }
                    }
                    
                }
                homeCats = homeCats.OrderBy(c => c.Level == CategoryLevel.Level1).ToList();
                return homeCats;
            });
        }
        public async Task<List<CategoryVm>> GetCategoryProducts()
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Category + "GetCategoryProducts", async () =>
            {
                var appSetting = await GetAppSetting();
                var categories = await GetAllCategories();
                var listId = appSetting.CategoryProducts != null ? appSetting.CategoryProducts.Split(",") : [];
                var proCats = new List<CategoryVm>();
                if (listId.Length > 0)
                {
                    foreach (var item in categories)
                    {
                        if (listId.Contains(item.Id.ToString()) && item.Type == CategoryType.Products)
                        {
                            proCats.Add(item);
                        }
                        foreach (var itemChild in item.ChildCategories)
                        {
                            if (listId.Contains(itemChild.Id.ToString()) && item.Type == CategoryType.Products)
                            {
                                proCats.Add(itemChild);
                            }
                            foreach (var itemChildLv3 in itemChild.ChildCategories)
                            {
                                if (listId.Contains(itemChildLv3.Id.ToString()) && itemChildLv3.Type == CategoryType.Products)
                                {
                                    proCats.Add(itemChildLv3);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in categories)
                    {
                        if (item.Type == CategoryType.Products)
                        {
                            proCats.Add(item);
                        }
                        foreach (var itemChild in item.ChildCategories)
                        {
                            if (itemChild.Type == CategoryType.Products)
                            {
                                proCats.Add(itemChild);
                            }
                            foreach (var itemChildLv3 in itemChild.ChildCategories)
                            {
                                if (itemChildLv3.Type == CategoryType.Products)
                                {
                                    proCats.Add(itemChildLv3);
                                }
                            }
                        }
                    }
                }
                proCats = proCats.OrderBy(c => c.Level == CategoryLevel.Level1).ToList();
                return proCats;
            });
        }
 
        public async Task<PagedResult<ArticleVm>> GetCategoryArticles(int id, int page)
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Article + "GetCategoryArticles_" + id + "_" + page, async () =>
            {
                var result = await _article.GetForCategory(id, page);
                return result;
            });
            
        }

        // Articles
        public async Task<List<ArticleVm>> GetArticlesByCategoryId(int categoryId)
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Article + "GetArticlesByCategoryId" + categoryId, async () =>
            {
                var articles = await _article.GetForCategory(categoryId, 1);
                return articles.Items;
            });
        }
        public async Task<List<ArticleVm>> GetArticlesFeddback()
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Article + "GetArticlesFeddback", async () =>
            {
                var appSetting = await GetAppSetting();
                var articles = await _article.GetArticleByIds(appSetting.Feedbacks);
                return articles.ToList();
            });

        }
        public async Task<List<ArticleVm>> GetArticlesHotNews()
        {
            return await _cache.GetOrSetAsync(SystemConstants.Cache_Article + "GetArticlesHotNews", async () =>
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
