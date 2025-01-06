using Dentistry.ViewModels.Catalog.Slide;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dentistry.Admin.Controllers
{
    public class SlideController : Controller
    {
        private readonly ISlideRepository _slideRepository;
        public SlideController(ISlideRepository slideRepository)
        {
            _slideRepository = slideRepository;
        }

        public async Task<IActionResult> Index()
        {
            var slides = await _slideRepository.GetAllAsync();
            var slideVms = slides.Select(x => x.ReturnViewModel()).ToList();
            return View(slideVms);
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

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id) { 
            var result = await _slideRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
