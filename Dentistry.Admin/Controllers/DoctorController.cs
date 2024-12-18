using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Common;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dentistry.Admin.Controllers
{
    public class DoctorController : BaseController
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
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id) {
            if (id == 0) {
                return PartialView("~/Views/Doctor/Partial/_addEdit.cshtml", new DoctorVm() { Dob = DateTime.Now });
            } else
            {
                var doctor = await _doctorRepository.GetById(id);
                return PartialView("~/Views/Doctor/Partial/_addEdit.cshtml", doctor.ReturnViewModel());
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(DoctorVm item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            if (item == null) return Json(new { });
            if (item.Id == 0)
            {
                var doctor = await _doctorRepository.Create(item);
            }
            else
            {
                var doctor = await _doctorRepository.Update(item);
            }
            return Json(new SuccessResult<bool>());
        }
    }
}
