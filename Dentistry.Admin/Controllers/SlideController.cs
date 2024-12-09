using Dentistry.Data.Services;
using Dentistry.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class SlideController : Controller
    {
        private readonly SlideService _slideService;
        public SlideController(SlideService slideService)
        {
            _slideService = slideService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edit(SlideVm model) {
            return PartialView("EditSlideComponent", model);        
        
        }
        [HttpGet]
        public IActionResult AddEdit()
        {
            var slide = new SlideVm();
            return View(slide);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult AddEdit([FromForm] SlideVm model)
        {
            var slide = _slideService.Create(model);
            return View(model);
        }
    }
}
