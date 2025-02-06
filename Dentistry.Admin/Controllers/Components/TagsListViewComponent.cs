using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers.Components
{
    public class TagsListViewComponent : ViewComponent
    {
        private readonly ITagsRepository _tagsRepository;

        public TagsListViewComponent(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }   
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var _tags = await _tagsRepository.GetAll();
            return View("~/Views/Tags/Partial/_listView.cshtml", _tags.Select(x => x.ReturnViewModel()).ToList());
        }
    }
}
