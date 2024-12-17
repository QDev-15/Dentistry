using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class DoctorController : Controller
    {

        private readonly IDoctorRepository _doctorRepository;
        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorRepository.GetAll();
            return View(doctors.Select(x=>x.ReturnViewModel()).ToList());
        }
        [HttpGet]
        public IActionResult List()
        {
            return ViewComponent("DoctorList");
        }
    }
}
