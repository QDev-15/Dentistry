using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Utilities.Slides;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class SlideController : Controller
    {
        private readonly ISlideRepository _slideRepository;
        public SlideController(ISlideRepository slideRepository)
        {
            _slideRepository = slideRepository;
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
        public async Task<IActionResult> AddEdit([FromForm] SlideVm model)
        {
            var slide = await _slideRepository.Create(model);
            return View(slide);
        }
    }
}
