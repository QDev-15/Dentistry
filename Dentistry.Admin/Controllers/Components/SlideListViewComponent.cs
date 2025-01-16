using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers.Components
{
    public class SlideListViewComponent : ViewComponent
    {
        private readonly ISlideRepository _slideRepository;

        public SlideListViewComponent(ISlideRepository slideRepository)
        {
            _slideRepository = slideRepository;
        }   
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slides = await _slideRepository.GetAllAsync();
            var slideVms = slides.Select(x => x.ReturnViewModel()).ToList();
            return View("~/Views/slide/Partial/_slideList.cshtml", slideVms);
        }
    }
}
