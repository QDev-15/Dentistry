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
        private readonly LoggerRepository _loggerRepository;

        public CategoryRepository(DentistryDbContext context, LoggerRepository loggerRepository) : base(context)
        {
            _loggerRepository = loggerRepository;
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetByParentId(int parentId)
        {
            var categories = await _context.Categories.Include(i => i.Parent).Include(i => i.Categories).Where(x => x.ParentId == parentId).ToListAsync();
            return categories;
        }
        public async Task<IEnumerable<Category>> GetByParent()
        {
            var categories = await _context.Categories.Include(i => i.Parent).Include(i => i.Categories).Where(x => x.ParentId == null || x.ParentId == 0).ToListAsync();
            return categories;
        }
        public async Task<IEnumerable<Category>> GetChilds()
        {
            var categories = await _context.Categories.Include(i => i.Parent).Include(i => i.Categories).Where(x => x.ParentId != null && x.ParentId > 0).ToListAsync();
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

        public async Task<CategoryVm> CreateNew(CategoryVm model)
        {
            try
            {
                var category = new Category()
                {
                    Alias = model.Alias,
                    Name = model.Name,
                    IsActive = model.IsActive,
                    ParentId = model.ParentId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    UserId = model.UserId ?? new Guid(_loggerRepository.GetCurrentUserId()),
                };
                await AddAsync(category);
                await SaveChangesAsync();
                return (await GetById(category.Id)).ReturnViewModel();
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message);
                return model;
            }
            
        }

        public async Task<CategoryVm> UpdateCategory(CategoryVm model)
        {
            if (model == null || model.Id == 0) return model;

            try
            {
                var category = await GetById(model.Id);
                if (category != null)
                {
                    category.Alias = model.Alias;
                    category.Name = model.Name;
                    category.IsActive = model.IsActive;
                    category.ParentId = model.ParentId;
                    category.UpdatedDate = DateTime.Now;
                    Update(category);
                    await SaveChangesAsync();
                }
            }
            catch (Exception ex) { 
                _loggerRepository.QueueLog(ex.Message);
            }

            return model;
        }

        public async Task<bool> CheckExistsAlias(string alias)
        {
            return await _context.Categories.AnyAsync(c => c.Alias == alias);
        }
    }
}
