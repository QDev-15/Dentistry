using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeSlideDoctorViewComponent : ViewComponent
    {
        private readonly IDoctorRepository _doctorRepository;
        public HomeSlideDoctorViewComponent(IDoctorRepository doctorRepository) { 
            _doctorRepository = doctorRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var docs = await _doctorRepository.GetDoctorForApplication();
            return View("~/Views/ViewComponents/HomeSlideDoctor.cshtml", docs);
        }
    }
    
}
