﻿using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface ICategoryReposiroty : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetChilds();
        Task<IEnumerable<Category>> GetParents();
        /// <summary>
        /// Get Category not get Parent and Childs
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<List<CategoryVm>> GetCategoryByType(CategoryType type);
        Task<Category> GetById(int id);
        Task<CategoryVm> GetByAlias(string alias);
        Task<CategoryVm> UpLoadFile(int id, IFormFile file);
        Task<CategoryVm> CreateNew(CategoryVm category);
        Task<CategoryVm> UpdateCategory(CategoryVm category);
        Task<IEnumerable<CategoryVm>> GetForSettings();
        /// <summary>
        /// If exixts return True, else return False
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        Task<bool> CheckExistsAlias(string alias, int id);
        Task<bool> DeleteAsync(Category category);
        void RefreshCategory();
    }
}
