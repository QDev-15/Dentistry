using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.Enums;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactRepository _contactRepository;
        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult List()
        {
            return ViewComponent("ContactList");
        }
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            if (id == 0)
            {
                return Json(new ErrorResult<bool>("Không tìm thấy yêu cầu."));
            }
            else
            {
                var item = await _contactRepository.GetById(id);

                return PartialView("~/Views/Contact/Partial/_view.cshtml", item);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Save(ContactVm model)
        {
            try
            {
                await _contactRepository.Update(model);
                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex) {
                return Json(new ErrorResult<bool>(ex.Message));
            }
        }
        [HttpGet]
        public async Task<IActionResult> Process(int id) {
            try
            {
                await _contactRepository.Process(id);
                return Json(new SuccessResult<bool>());
            } catch (Exception ex)
            {
                return Json(new ErrorResult<bool>(ex.Message));
            }
        }
    }
}
