using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            var categories = await _context.Categories.Where(x => x.ParentId == parentId).ToListAsync();
            return categories;
        }
    }
}
