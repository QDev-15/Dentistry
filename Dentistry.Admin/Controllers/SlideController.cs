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
            var slide = await _slideRepository.GetByIdAsync(id);
            return View(slide.ReturnViewModel());
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddEdit([FromForm] SlideVm model)
        {
            var slide = await _slideRepository.Create(model);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id) { 
            var result = await _slideRepository.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(int id, string name, int age)
        {
            //var user = _context.Users.FirstOrDefault(u => u.Id == id);

            //if (user == null)
            //{
            //    return Json(new { success = false });
            //}

            //// Cập nhật thông tin người dùng
            //user.Name = name;
            //user.Age = age;

            //_context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
