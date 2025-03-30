using Dentistry.ViewModels.Catalog.Contacts;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers.Components
{
    public class ContactListViewComponent : ViewComponent
    {
        private IContactRepository _contactRepository;
        public ContactListViewComponent(IContactRepository contactRepository) {
            _contactRepository = contactRepository;        
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = new ContactVmList();
            var contactsActive = await _contactRepository.GetAll(true);
            var contactsInActive = await _contactRepository.GetAll(false);
            result.ContactActives = contactsActive.Select(x => x.ReturnViewModel()).ToList();
            result.ContactInActives = contactsInActive.Select(x => x.ReturnViewModel()).ToList();
            return View("~/Views/Contact/Partial/_contact_list.cshtml", result);
        }
    }
}
