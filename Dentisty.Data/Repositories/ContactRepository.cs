﻿using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentisty.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        private readonly DentistryDbContext _context;
        private readonly IImageRepository _imageRepository;
        private readonly LoggerRepository _loggerRepository;
        public ContactRepository(DentistryDbContext context, IImageRepository imageRepository, LoggerRepository loggerRepository) : base(context)
        {
            _context = context;
            _imageRepository = imageRepository;
            _loggerRepository = loggerRepository;   
        }

        public async Task<ContactVm> Create(ContactVm vm)
        {
            try
            {
                var contact = new Contact()
                {
                    CreatedDate = DateTime.Now,
                    Email = vm.Email,
                    IsActive = vm.IsActive,
                    Message = vm.Message,
                    Name = vm.Name,
                    PhoneNumber = vm.PhoneNumber,
                    UpdatedDate = DateTime.Now,
                };
                await AddAsync(contact);
                await SaveChangesAsync();
                return contact.ReturnViewModel();
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message, "Contact");
                return null;
            }
            
        }

        public async Task<IEnumerable<Contact>> GetByEmail(string email)
        {
            return await _context.Contacts.Where(x => x.Email == email).ToListAsync();
        }

        public async Task<IEnumerable<Contact>> GetByPhone(string phone)
        {
            return await _context.Contacts.Where(x => x.PhoneNumber == phone).ToListAsync();
        }

        public async Task<ContactVm> Update(ContactVm vm)
        {
            try
            {
                var contact = await GetByIdAsync(vm.Id);
                if (contact != null)
                {
                    contact.IsActive = vm.IsActive;
                    contact.UpdatedDate = DateTime.Now;
                    contact.ProcessById = new Guid(_loggerRepository.GetCurrentUserId());
                    Update(contact);
                    await SaveChangesAsync();
                }
                return vm;
                
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message);
                return null;
            }
        }
    }
}