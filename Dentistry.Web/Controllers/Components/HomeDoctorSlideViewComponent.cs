using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeDoctorSlideViewComponent : ViewComponent
    {
        private readonly ApplicationService _app;
        public HomeDoctorSlideViewComponent(ApplicationService app) {
            _app = app;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var docs = await _app.GetDoctorSlides();
            return View("~/Views/ViewComponents/HomeDoctorSlide.cshtml", docs);
        }
    }
    
}
