using Dentistry.Common;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.Enums;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly LoggerRepository _loggerRepository;
        public ArticleController(IArticleRepository articleRepository, LoggerRepository loggerRepository) {
            _articleRepository = articleRepository;
            _loggerRepository = loggerRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("bai-viet/{alias}")]
        public async Task<IActionResult> Articles(string alias)
        {
            var baivietDetail = new ArticleDetailVm();
            try
            {
                var baiviet = await _articleRepository.GetByAliasAsync(alias);
                if (baiviet == null) { 
                    return NotFound("bai viet not found: alias = " + alias);
                }
                var baiviets = await _articleRepository.GetForApplication(ArticleType.Article);
                baivietDetail.item = baiviet.ReturnViewModel();
                baivietDetail.items = baiviets.Where(x => x.Id != baiviet.Id).ToList();

                ViewData["Title"] = baiviet.Title;
                ViewData["Description"] = $"Đọc ngay bài viết '{baiviet.Title}' để hiểu hơn về {baiviet.Tags}";
                ViewData["Keywords"] = baiviet.Tags;
                return View(baivietDetail);
            } catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message, "get article alias = " + alias, "client");
                // Tránh crash app
                return StatusCode(500, "Lỗi server");
            }

            
            
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _articleRepository.DeleteArticle(id);
                return Json(new SuccessResult<bool>());
            } catch(Exception ex)
            {
                return Json(new ErrorResult<bool>() { Message = ex.Message });
            }
        }
        
        
    }
}
