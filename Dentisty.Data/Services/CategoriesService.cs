using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Interfaces;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Utilities.Slides;
using Dentisty.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.Data.Services
{
    public class CategoriesService
    {
        private readonly IRepository<Category> _repository;
        private readonly DentistryDbContext _context;

        public CategoriesService(DentistryDbContext context, IRepository<Category> repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryVm>> GetAll()
        {
            var categories = await _context.Categories.Select(x => new CategoryVm
            {
                Id = x.Id,
                Name = x.Name,
                Alias = x.Alias,
                Parent = x.Parent != null ? new CategoryVm { Id = x.Parent.Id, Name = x.Parent.Name } : null,
                ChildCategories = x.Categories.Select(a => new CategoryVm
                {
                    Id = a.Id,
                    Name = a.Name,
                    Alias = a.Alias
                }).ToList()
            }).ToListAsync();

            return categories;
        }
        public async Task<CategoryVm> GetById(int id)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);
            return new CategoryVm()
            {
                Id = id,
                Name = category.Name,
                Parent = category.Parent != null ? new CategoryVm { Id = category.Parent.Id, Name = category.Parent.Name } : null,
                Alias = category.Alias,
                ChildCategories = category.Categories.Select(a => new CategoryVm
                {
                    Id = a.Id,
                    Name = a.Name,
                    Alias = a.Alias
                }).ToList()
            };
        }
    }
}