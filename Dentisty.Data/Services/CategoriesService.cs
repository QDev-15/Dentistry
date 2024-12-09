using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Utilities.Slides;
using Dentisty.Data.Interfaces;
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
        private readonly CategoryRepository _categoryRepository;

        public CategoriesService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryVm>> GetAll()
        {
            var categories = await _categoryRepository.GetAll();

            if (categories == null || !categories.Any())
            {
                return new List<CategoryVm>();
            }

            var items = new List<CategoryVm>();
            foreach (var item in categories.ToList())
            {
                var newCategory = new CategoryVm()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Alias = item.Alias
                };
                if (item.Parent != null)
                {
                    newCategory.Parent = new CategoryVm()
                    {
                        Id = item.Parent.Id,
                        Name = item.Parent.Name,
                        Alias = item.Parent.Alias,
                        ParentId = item.Parent.Id
                    };
                }
                if (item.Categories != null && item.Categories.Count > 0)
                {
                    newCategory.ChildCategories = new List<CategoryVm>();
                    foreach (var cat in item.Categories)
                    {
                        newCategory.ChildCategories.Add(new CategoryVm()
                        {
                           Id = cat.Id,
                           Alias = cat.Alias,
                           Name = cat.Name,
                           ParentId = cat.ParentId
                        });
                    }
                }
                items.Add(newCategory);
            }

            return items;
        }
        public async Task<CategoryVm> GetById(int id)
        {
            var category = await _categoryRepository.GetById(id);
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