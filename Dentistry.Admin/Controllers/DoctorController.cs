using Dentistry.Common;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Common;
using Dentisty.Data;
using Dentisty.Common;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class DoctorController : BaseController
    {

        private readonly IDoctorRepository _doctorRepository;
        private readonly CacheNotificationService _cacheNotificationService;
        public DoctorController(IDoctorRepository doctorRepository, CacheNotificationService cacheNotificationService)
        {
            _cacheNotificationService = cacheNotificationService;
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
            var checkAlis = await _doctorRepository.CheckExistsAlias(item.Alias, item.Id);
            if (item.Alias == null || checkAlis)
            {
                checkAlis = await _doctorRepository.CheckExistsAlias(item.Name.ToSlus(), item.Id);
                if (checkAlis)
                {
                    item.Alias = item.Name.ToSlus() + "-" + Helper.GenerateRandomString(4);
                } else
                {
                    item.Alias = item.Name.ToSlus();
                }
            }
            if (item.Id == 0)
            {
                var doctor = await _doctorRepository.Create(item);
            }
            else
            {
                var doctor = await _doctorRepository.Update(item);
            }
            await _cacheNotificationService.InvalidateCacheAsync(SystemConstants.Cache_Doctor);
            return Json(new SuccessResult<bool>());
        }
    }
}
