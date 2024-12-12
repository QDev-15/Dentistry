using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface ICategoryReposiroty : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetByParentId(int parentId);
        Task<IEnumerable<Category>> GetByParent();
        Task<Category> GetById(int id);
        Task<CategoryVm> CreateNew(CategoryVm category);
        Task<CategoryVm> UpdateCategory(CategoryVm category);
        /// <summary>
        /// If exixts return True, else return False
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        Task<bool> CheckExistsAlias(string alias);
    }
}
