using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Common;
using Dentisty.Data.Interfaces;
using Dentisty.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dentistry.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly ApplicationService _app;
        public DoctorController(IDoctorRepository doctorRepository, ApplicationService app)    
        {
            _doctorRepository = doctorRepository;
            _app = app;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("bac-si/{alias}")]
        public async Task<IActionResult> Detail(string alias) {
            try
            {
                DoctorDetailVm detail = new DoctorDetailVm();
                var doctor = await _doctorRepository.GetByAlias(alias);
                var doctors = await _doctorRepository.GetDoctorForApplication();
                detail.Doctor = doctor;
                detail.Doctors = doctors.Where(x => x.Id != doctor.Id).ToList();
                var title = await _app.GetApplicationName() + " - " + doctor.Name;
                ViewData["Title"] = title;
                ViewData["Description"] = doctor.PositionExtent;
                ViewData["Keywords"] = doctor.Name;
                ViewData["Image"] = doctor.AvatarPath;


                return View(detail);
            } catch(Exception ex)
            {
                return Json(new ErrorResult<bool>()
                {
                    Message = ex.Message
                });
            }
            
        }
    }
}
