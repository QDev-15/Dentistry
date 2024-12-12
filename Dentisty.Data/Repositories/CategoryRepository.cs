using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Categories;
using Dentisty.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Dentisty.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryReposiroty
    {
        private readonly DentistryDbContext _context;

        public CategoryRepository(DentistryDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetByParentId(int parentId)
        {
            var categories = await _context.Categories.Include(i => i.Parent).Include(i => i.Categories).Where(x => x.ParentId == parentId).ToListAsync();
            return categories;
        }
        public async Task<Category> GetById(int id)
        {
            var category = await _context.Categories.Include(i => i.Parent).Include(i => i.Categories).FirstOrDefaultAsync(x => x.Id == id);
            return category;
        }
        public new async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = await _context.Categories.Include(x=>x.Parent).Include(x=>x.Categories).ToListAsync();
            return categories;
        }

        public Task<Category> CreateNew(CategoryVm category)
        {
            throw new NotImplementedException();
        }
    }
}
