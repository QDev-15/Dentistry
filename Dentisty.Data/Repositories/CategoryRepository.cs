using Dentistry.Common;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.Enums;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Dentisty.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryReposiroty
    {
        private readonly DentistryDbContext _context;
        private readonly LoggerRepository _loggerRepository;
        private readonly IImageRepository _imageRepository;

        public CategoryRepository(DentistryDbContext context, IImageRepository imageRepository , LoggerRepository loggerRepository) : base(context)
        {
            _loggerRepository = loggerRepository;
            _context = context;
            _imageRepository = imageRepository;
        }

        public async Task<IEnumerable<Category>> GetByParentId(int parentId)
        {
            var categories = await _context.Categories.Where(x => x.IsActive == true && x.ParentId == parentId).Include(i => i.Image).Include(i => i.Parent).Include(i => i.Categories).ToListAsync();
            return categories;
        }

        public async Task<Category> GetById(int id)
        {
            var category = await _context.Categories.Where(x => x.IsActive == true && x.Id == id).Include(i => i.Image).Include(i => i.Parent).Include(i => i.Categories).FirstOrDefaultAsync();
            return category;
        }
        public async Task<CategoryVm> GetByAlias(string alias)
        {
            var category = await _context.Categories.Where(x => x.IsActive == true && x.Alias.ToString() == alias.ToString())
                .Include(i => i.Image)
                .Include(i => i.Parent)
                .Include(i => i.Categories)
                .ThenInclude(x => x.Image).FirstOrDefaultAsync();
            return category.ReturnViewModel();
        }
        public new async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = await _context.Categories.Where(x => x.IsActive).Include(i => i.Image).Include(x=>x.Parent).Include(x=>x.Categories).ToListAsync();
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
                    IsActive = true,
                    IsParent = model.IsParent,
                    ParentId = model.ParentId == 0 ? null : model.ParentId,
                    Position = model.Position,
                    Level  = model.Level,
                    Type = model.Type,
                    Sort = model.Sort,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    UserId = model.UserId ?? new Guid(_loggerRepository.GetCurrentUserId()),
                };
                if (model.ParentId > 0)
                {
                    category.Parent = await _context.Categories.Where(x => x.IsActive == true && x.Id == model.ParentId).FirstOrDefaultAsync();
                    category.Type = category.Parent.Type;
                }
                await AddAsync(category);
                await SaveChangesAsync();
                if (model.ImageFile != null)
                {
                    var image = await CreateImageAsync(model.ImageFile);
                    category.Image = image;
                    UpdateAsync(category);
                    await SaveChangesAsync();
                }
                return category.ReturnViewModel();
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message);
                throw new Exception(ex.Message);
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
                    if (category.Parent == null)
                    {
                        category.Level = CategoryLevel.Level1;
                    } else if (category.Level != CategoryLevel.Level2 && category.Parent !=null && category.Parent.Parent == null )
                    {
                        category.Level = CategoryLevel.Level2;
                    } else
                    {
                        category.Level = CategoryLevel.Level3;
                    }
                    category.Alias = model.Alias;
                    category.Name = model.Name;
                    category.Sort = model.Sort;
                    category.Description = model.Description;
                    category.Position = model.Position;
                    category.Type = model.Type;
                    if (category.Level != CategoryLevel.Level1 && category.Parent != null)
                    {
                        category.Type = category.Parent.Type;
                    }
                    category.UpdatedDate = DateTime.Now;
                    UpdateAsync(category);
                    await SaveChangesAsync();
                }
                if (model.ImageFile != null)
                {
                    var image = await CreateImageAsync(model.ImageFile);
                    // delete old image
                    if (category.Image != null) {
                        await _imageRepository.DeleteFile(category.Image);
                        category.Image = null;
                        UpdateAsync(category);
                        await SaveChangesAsync();
                    }
                    // add new Image
                    category.Image = image;
                    UpdateAsync(category);
                    await SaveChangesAsync();
                }
                return category.ReturnViewModel();
            }
            catch (Exception ex) { 
                _loggerRepository.QueueLog(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CheckExistsAlias(string alias, int id)
        {
            if (string.IsNullOrEmpty(alias)) return true;
            return await _context.Categories.Where(x => x.IsActive == true).AnyAsync(c => c.Alias == alias && c.Id != id);
        }
        private async Task<ImageFile> CreateImageAsync(IFormFile formFile)
        {
            if (formFile == null) { return null; }
            
            var image = await _imageRepository.CreateAsync(formFile, SystemConstants.Folder.Category);
            return image;
        }

        public async Task<IEnumerable<Category>> GetRightMenuAsync()
        {
            var rightCategories = await _context.Categories.Where(x => x.IsActive == true && x.Position == CategoryPosition.MenuRight).Include(x => x.Categories).ToListAsync();
            return rightCategories;
        }

        public async Task<IEnumerable<Category>> GetLeftMenuAsync()
        {
            var rightCategories = await _context.Categories.Where(x => x.IsActive == true && x.Position == CategoryPosition.MenuLef).Include(x => x.Categories).ToListAsync();
            return rightCategories;
        }

        public async Task<IEnumerable<CategoryType>> GetCategoryParentTypes()
        {
            var categorys = await _context.Categories.Where(x => x.IsActive == true && x.Level == CategoryLevel.Level1).Select(x => x.Type ?? CategoryType.None).Distinct().ToListAsync();
            return categorys;
        }
        public async Task<IEnumerable<Category>> GetParents()
        {
            var categories = await _context.Categories
                .Where(x => x.IsActive == true && (x.Level == CategoryLevel.Level1 || x.ParentId == null))
                .Include(c => c.Image) // Ảnh của cấp 1
                .Include(c => c.Categories.Where(sub => sub.IsActive == true)) // Lấy cấp 2
                    .ThenInclude(sub => sub.Image) // Ảnh của cấp 2
                .Include(c => c.Categories) // Đảm bảo cấp 2 được Include trước khi lấy cấp 3
                    .ThenInclude(sub => sub.Categories.Where(subsub => subsub.IsActive == true)) // Lấy cấp 3
                        .ThenInclude(subsub => subsub.Image) // Ảnh của cấp 3
                .ToListAsync();

            return categories;
        }
        public async Task<IEnumerable<Category>> GetChilds()
        {
            var categories = await _context.Categories.Where(x => x.IsActive == true && x.Level != CategoryLevel.Level1).Include(i => i.Image).Include(i => i.Parent).Include(i => i.Categories).ToListAsync();
            return categories;
        }

        public async Task<IEnumerable<CategoryVm>> GetForSettings()
        {
            return await _context.Categories.Where(x => x.IsActive).OrderBy(x => x.Level == CategoryLevel.Level1).Select(x => new CategoryVm() { Id = x.Id, Name = x.Name, Type = x.Type }).ToListAsync();
        }

        public async Task<IEnumerable<CategoryVm>> GetFlatHomePage()
        {
            // get settings
            var settting = (await _context.AppSettings.FirstOrDefaultAsync(x => x.Id == 1)).ReturnViewModel();
            if (settting.ShowCategoryList && settting.Categories.Length > 0) {
                var categoryIds = settting.Categories.Split(',');
                var categories = await _context.Categories.Where(x => x.IsActive == true && categoryIds.Contains(x.Id.ToString())).Take(8).Include(x => x.Image).ToListAsync();
                return categories.Select(x => x.ReturnViewModel());
            }
            return new List<CategoryVm>();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category != null)
                {
                    await _imageRepository.DeleteFile(category.Image);
                    category.Name += Guid.NewGuid().ToString();
                    category.Alias = category.Name.ToSlus();
                    category.IsActive = false;
                    UpdateAsync(category);
                    await SaveChangesAsync();
                    return true;
                }
                throw new Exception("Danh mục không tồn tại.");
            } catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message, "Delete Category Id = " + id);
                throw new Exception(ex.Message);
            }
            
        }

        public async void RefreshCategory()
        {
            try
            {
                var categories = _context.Categories
                    .Include(x => x.Parent)
                    .ThenInclude(x => x.Parent)
                    .ThenInclude(x=> x.Parent)
                    .ToList();
                if (categories.Any())
                {
                    foreach (var category in categories) {
                        if (category == null) continue;
                        // level1
                        if (category.Parent == null) {
                            category.Level = CategoryLevel.Level1;
                        }
                        else // level2
                        if (category.Parent.Parent == null)
                        {
                            category.Level = CategoryLevel.Level2;  
                            category.Type = category.Parent.Type;
                        }
                        else // level3
                        {
                            category.Level = CategoryLevel.Level3;
                            category.Type = category.Parent?.Parent?.Type;
                        }
                        _context.Update(category);
                    }
                }
                int affectedRows = _context.SaveChanges();
                _loggerRepository.QueueLog($"Đã cập nhật {affectedRows} categories.", "Refresh category");
            }
            catch (Exception ex) {
                _loggerRepository.QueueLog(ex.Message, "Error Refresh category");
            }
            
        }
    }
}
