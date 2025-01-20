using Dentisty.Data.Repositories;
using Dentistry.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Mvc;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentisty.Data.Interfaces;
using Dentistry.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Dentistry.Web.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactRepository _contactRepository;
        public ContactController(IContactRepository contactRepository, ICompositeViewEngine viewEngine):base(viewEngine) { 
            _contactRepository = contactRepository; 
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(ContactVm model)
        {
            if (!ModelState.IsValid)
            {
                // Render HTML từ PartialView và trả về trong JSON
                var partialViewHtml = await RenderViewToStringAsync("Views/Contact/Partials/AddMessage.cshtml", model);
                return Json(new ErrorResult<string>
                {
                    data = partialViewHtml,
                    Message = "Validation failed"
                });
            }

            try
            {
                var contact = await _contactRepository.Create(model);
                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex) { 
                return Json(new ErrorResult<bool> { Message = ex.Message });
            }
        }

    }
}
