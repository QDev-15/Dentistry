using Dentisty.Data.Common;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Dentisty.Data.Interfaces;
using Dentistry.ViewModels.Catalog.Tags;
using Dentisty.Data;

namespace Dentistry.Admin.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagsRepository _tagsRepository;
        public TagsController(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult List()
        {
            return ViewComponent("TagsList");
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            if (id == 0)
            {
                return PartialView("~/Views/Tags/Partial/_addEdit.cshtml", new TagsVm());
            }
            else
            {
                var tag = await _tagsRepository.GetById(id);
                return PartialView("~/Views/Tags/Partial/_addEdit.cshtml", tag.ReturnViewModel());
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(TagsVm item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            
            if (item.Id == 0)
            {
                var doctor = await _tagsRepository.Create(item);
            }
            else
            {
                var doctor = await _tagsRepository.Update(item);
            }
            return Json(new SuccessResult<bool>());
        }
    }
}
