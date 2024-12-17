using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers.Components
{
    public class DoctorListViewComponent : ViewComponent
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorListViewComponent(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }   
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var _doctors = await _doctorRepository.GetAll();
            return View("~/Views/Doctor/Partial/_list.cshtml", _doctors.Select(x => x.ReturnViewModel()).ToList());
        }
    }
}
