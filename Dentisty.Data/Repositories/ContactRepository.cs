using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class ContactRepository
    {
        private readonly IRepository<Contact> _repositoryContact;

        public ContactRepository(IRepository<Contact> repositoryContact)
        {
            _repositoryContact = repositoryContact;
        }

        public async Task<Contact> GetById(int id)
        {
            var contact = await _repositoryContact.GetByIdAsync(id);
            return contact;
        }
        public async Task<IEnumerable<Contact>> GetAll()
        {
            return await _repositoryContact.GetAllAsync();
        }
        public async Task<Contact> Create(Contact contact)
        {
            await _repositoryContact.AddAsync(contact);
            await _repositoryContact.SaveChangesAsync();
            return contact;
        }
        public async void Update(Contact contact) {
            _repositoryContact.Update(contact);
            await _repositoryContact.SaveChangesAsync();
        }
    }
}
