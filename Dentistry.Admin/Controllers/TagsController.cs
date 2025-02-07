using Dentisty.Data.Common;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Dentisty.Data.Interfaces;
using Dentistry.ViewModels.Catalog.Tags;
using Dentisty.Data;
using Dentisty.Data.Repositories;

namespace Dentistry.Admin.Controllers
{
    public class TagsController : BaseController
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly LoggerRepository _logs;
        public TagsController(ITagsRepository tagsRepository, LoggerRepository loggerRepository)
        {
            _logs = loggerRepository;
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
        public async Task<IActionResult> AddEditTag(TagsVm item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            
            try
            {
                if (item.Id == 0)
                {
                    var doctor = await _tagsRepository.Create(item);
                } else
                {
                    var doctor = await _tagsRepository.Update(item);
                }
                return Json(new SuccessResult<bool>());
            } catch(Exception ex)
            {
                _logs.QueueLog(ex.Message, "AddEdit Tags");
                return Json(new ErrorResult<bool>() { Message = ex.Message }); 
            }

            
            
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _tagsRepository.Delete(id);
                return Json(new SuccessResult<bool>());
            } catch (Exception ex)
            {
                _logs.QueueLog(ex.Message, "Delete Tags");
                return Json(new ErrorResult<bool>() { Message = "Lỗi xảy ra. Vui lòng thử lại sau." });
            }
        }
    }
}
