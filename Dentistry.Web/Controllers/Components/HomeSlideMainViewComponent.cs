using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeSlideMainViewComponent : ViewComponent
    {
        private readonly ISlideRepository _slideRepository;
        public HomeSlideMainViewComponent(ISlideRepository slideRepository) { 
            _slideRepository = slideRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slides = await _slideRepository.GetActiveSlides();
            return View("~/Views/ViewComponents/HomeSlideMain.cshtml", slides);
        }
    }
    
}
