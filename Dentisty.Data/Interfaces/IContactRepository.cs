using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<IEnumerable<Contact>> GetByEmail(string email);
        Task<IEnumerable<Contact>> GetByPhone(string phone);
        Task<ContactVm> Update(ContactVm vm);
        Task<ContactVm> Create(ContactVm vm);
    }
}
