using Dentistry.ViewModels.Common;
using Dentisty.Data.Interfaces;
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
        public async Task<IActionResult> Process(int id) {
            
            await _contactRepository.Process(id);
            return Json(new SuccessResult<bool>());
        }
    }
}
