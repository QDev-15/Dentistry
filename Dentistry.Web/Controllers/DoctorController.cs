using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Common;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dentistry.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        public DoctorController(IDoctorRepository doctorRepository)    
        {
            _doctorRepository = doctorRepository;
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

                ViewData["Title"] = doctor.Name;
                ViewData["Description"] = $"Trang chủ Nhiên Nha Khoa - Cơ sở uy tín trao gửi niềm tin.";
                ViewData["Keywords"] = "Nhiên, Nha Khoa, Cơ sở uy tín, làm răng, răng sứ";


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
