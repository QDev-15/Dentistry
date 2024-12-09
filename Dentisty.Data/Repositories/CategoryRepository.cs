using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class CategoryRepository
    {
        private readonly IRepository<Category> _repository;

        public CategoryRepository(IRepository<Category> repository)
        {
            _repository = repository;
        }
        public async Task<Category> GetById(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category;
        }
        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<IEnumerable<Category>> GetByParentId(int parentId)
        {
            var categories = await _repository.GetAllAsync();
            var category = categories.Where(x => x.ParentId == parentId);
            return category;
        }
    }
}
