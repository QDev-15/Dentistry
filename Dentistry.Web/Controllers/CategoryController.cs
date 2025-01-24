using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Enums;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dentistry.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryReposiroty _categoryReposiroty;
        private readonly IArticleRepository _articleRepository;
        public CategoryController(ICategoryReposiroty categoryReposiroty, IArticleRepository articleRepository)
        {
            _categoryReposiroty = categoryReposiroty;
            _articleRepository = articleRepository; 
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{danhmuc}/{alias}")]
        public async Task<IActionResult> Detail(string danhmuc, string alias)
        {
            var categoryDetailVm = new CategoryDetailVm();
            CategoryType categoryType = CategoryType.None;

            switch (danhmuc)
            {
                case var _ when danhmuc == CategoryType.About.GetAliasDisplayName(): // Giới thiệu
                    categoryType = CategoryType.About;
                    break; 
                case var _ when danhmuc == CategoryType.advise.GetAliasDisplayName(): // Tư vấn
                    categoryType |= CategoryType.advise;
                    break; 
                case var _ when danhmuc == CategoryType.FeedBack.GetAliasDisplayName(): // Phản hồi
                    categoryType &= ~CategoryType.FeedBack;
                    break; 
                case var _ when danhmuc == CategoryType.News.GetAliasDisplayName(): // Tin tức
                    categoryType ^= CategoryType.News;
                    break; 
                case var _ when danhmuc == CategoryType.Products.GetAliasDisplayName(): // sản phẩm
                    categoryType = CategoryType.Products;
                    break;                                                                    
                case var _ when danhmuc == CategoryType.Service.GetAliasDisplayName(): // dịch vụ
                    categoryType = CategoryType.Service;
                    break;                                                                    
                case var _ when danhmuc == CategoryType.Support.GetAliasDisplayName(): // Hỗ trợ
                    categoryType = CategoryType.Support;
                    break;                                                                    
                default:
                    break;
            }
            if (categoryType == CategoryType.None) return View(categoryDetailVm);
            categoryDetailVm.category = await _categoryReposiroty.GetByAlias(alias);
            categoryDetailVm.articles = (await _articleRepository.GetByCategoryId(categoryDetailVm.category.Id)).ToList();
            return View(categoryDetailVm);
        }
    }
}
