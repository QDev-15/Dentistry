using Dentistry.Common;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.Common;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dentistry.Admin.Controllers
{
    public class SlideController : Controller
    {
        private readonly ISlideRepository _slideRepository;
        private readonly CacheNotificationService _cacheNotificationService;
        public SlideController(ISlideRepository slideRepository, CacheNotificationService cacheNotificationService)
        {
            _cacheNotificationService = cacheNotificationService;
            _slideRepository = slideRepository;
        }

        public async Task<IActionResult> Index()
        {
            var slides = await _slideRepository.GetAllAsync();
            var slideVms = slides.Select(x => x.ReturnViewModel()).ToList();
            return View(slideVms);
        }

        [HttpGet]
        public IActionResult List()
        {
            return ViewComponent("SlideList");
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            if (id == 0)
            {
                return View("~/Views/Slide/Partial/_addEditSlide.cshtml", new SlideVm());
            }
            var slide = await _slideRepository.GetByIdAsync(id);
            return View("~/Views/Slide/Partial/_addEditSlide.cshtml", slide.ReturnViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(SlideVm model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            try
            {
                if (model.Id == 0)
                {
                    // Add slide logic
                    var slide = await _slideRepository.Create(model);
                }
                else
                {
                    // Update slide logic
                    var slide = await _slideRepository.UpdateSlide(model);
                }
                await _cacheNotificationService.InvalidateCacheAsync(SystemConstants.CacheKeys.AppSlide);
                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex) {
                return Json(new ErrorResult<bool>(ex.Message));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            try
            {
                await _slideRepository.Delete(id);
                await _cacheNotificationService.InvalidateCacheAsync(SystemConstants.CacheKeys.AppSlide);
                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex) {
                return Json(new ErrorResult<bool>(ex.Message));
            }
        }
    }
}
