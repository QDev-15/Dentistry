using Dentistry.Common;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Common;
using Dentisty.Data;
using Dentisty.Common;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Dentisty.Data.Repositories;
using Dentistry.Data.GeneratorDB.Entities;

namespace Dentistry.Admin.Controllers
{
    public class DoctorController : BaseController
    {

        private readonly IDoctorRepository _doctorRepository;
        private readonly IImageRepository _imageRepository;
        private readonly CacheNotificationService _cacheNotificationService;
        public DoctorController(IDoctorRepository doctorRepository, CacheNotificationService cacheNotificationService, IImageRepository imageRepository )
        {
            _cacheNotificationService = cacheNotificationService;
            _doctorRepository = doctorRepository;
            _imageRepository = imageRepository;
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
            var result = new SuccessResult<bool>();
            if (item.Id == 0)
            {
                var doctor = await _doctorRepository.Create(item);
                result.data = doctor;
            }
            else
            {
                var doctor = await _doctorRepository.Update(item);
                result.data = doctor;
            }
            await _cacheNotificationService.InvalidateCacheAsync(SystemConstants.Cache_Doctor);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(int id, IFormFile? imageFile, IFormFile? backgroundFile)
        {
            if ((imageFile == null || imageFile.Length == 0) && (backgroundFile == null || backgroundFile.Length == 0))
            {
                return BadRequest(new { isSuccessed = false, message = "Không có ảnh được tải lên" });
            }
            var result = new SuccessResult<bool>();
            if (id > 0)
            {
                var doctorUpdate = await _doctorRepository.UpLoadFile(id, imageFile, backgroundFile);
                result.data = doctorUpdate;
            }


            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var doctor = await _doctorRepository.GetById(id);
                if (doctor != null && doctor.Avatar !=null)
                {
                    await _imageRepository.DeleteFile(doctor.Avatar);
                    _imageRepository.DeleteAsync(doctor.Avatar);
                    
                }
                _doctorRepository.DeleteAsync(doctor);
                await _doctorRepository.SaveChangesAsync();
                await _cacheNotificationService.InvalidateCacheAsync(SystemConstants.Cache_Category);
                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex)
            {
                return Json(new ErrorResult<bool>() { Message = ex.Message });
            }
        }
    }
}
