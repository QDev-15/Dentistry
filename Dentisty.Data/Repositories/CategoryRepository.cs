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

        public async Task<Category> GetById(int id)
        {
            var category = await _context.Categories.Where(x => x.IsActive && x.Id == id)
                .Include(i => i.Image)
                .Include(i => i.Parent)
                .Include(i => i.Categories.Where(x => x.IsActive))
                .FirstOrDefaultAsync();
            return category;
        }
        public async Task<CategoryVm> UpLoadFile(int id, IFormFile file)
        {
            var category = await _context.Categories.Where(x => x.IsActive && x.Id == id).Include(x => x.Image).FirstOrDefaultAsync();
            if (category != null)
            {
                var imageOld = category.Image;
                if (file != null)
                {
                    var image = await _imageRepository.CreateAsync(file, SystemConstants.Folder.Category);
                    category.Image = image;
                    // delete oldImage
                    if (imageOld != null)
                    {
                        _imageRepository.DeleteFileToHostingAsync(imageOld);
                        _imageRepository.DeleteAsync(imageOld);
                    }
                    UpdateAsync(category);
                    await SaveChangesAsync();
                }
            }
            return category.ReturnViewModel();
        }
        public async Task<CategoryVm> GetByAlias(string alias)
        {
            var category = await _context.Categories.Where(x => x.IsActive && x.Alias.ToString() == alias.ToString())
                .Include(i => i.Parent)
                .Include(i => i.Categories.Where(x => x.IsActive))
                .Include(i => i.Articles.Where(x => x.IsActive).Take(10))
                    .ThenInclude(x => x.Images)
                .FirstOrDefaultAsync();
            return category.ReturnViewModel();
        }
        public new async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = await _context.Categories.Where(x => x.IsActive)
                .Include(i => i.Image)
                .Include(x=>x.Parent)
                .Include(x=>x.Categories)
                .ToListAsync();
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
                    category.Alias = model.Alias;
                    category.Name = model.Name;
                    category.Sort = model.Sort;
                    category.Description = model.Description;
                    category.Position = model.Position;
                    category.Type = model.Type;
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
            return await _context.Categories.Where(x => x.IsActive).AnyAsync(c => c.Alias == alias && c.Id != id);
        }
        private async Task<ImageFile> CreateImageAsync(IFormFile formFile)
        {
            if (formFile == null) { return null; }
            
            var image = await _imageRepository.CreateAsync(formFile, SystemConstants.Folder.Category);
            return image;
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
            var categories = await _context.Categories.Where(x => x.IsActive == true && x.Level != CategoryLevel.Level1).Include(i => i.Image)
                .Include(i => i.Parent)
                .Include(i => i.Categories.Where(x => x.IsActive)).ToListAsync();
            return categories;
        }

        public async Task<IEnumerable<CategoryVm>> GetForSettings()
        {
            return await _context.Categories.Where(x => x.IsActive).OrderBy(x => x.Level == CategoryLevel.Level1).Select(x => new CategoryVm() { Id = x.Id, Name = x.Name, Type = x.Type }).ToListAsync();
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            try
            {
                if (category != null)
                {
                    await DeleteCategoryAsync(category);
                    await SaveChangesAsync();
                    return true;
                }
                throw new Exception("Danh mục không tồn tại.");
            } catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message, "Delete Category Id = " + category.Id);
                throw new Exception(ex.Message);
            }
            
        }

        private async Task DeleteCategoryAsync(Category category)
        {
            await _imageRepository.DeleteFile(category.Image);
            category.IsActive = false;
            category.Name += Guid.NewGuid().ToString();
            category.Alias = category.Name.ToSlus();
            var categories = _context.Categories.Where(x => x.ParentId == category.Id).Include(x => x.Image).ToList();
            if (categories.Any()) {
                //
                foreach (var item in categories)
                {
                    await DeleteCategoryAsync(item);
                }
            }
            UpdateAsync(category);
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

        public async Task<List<CategoryVm>> GetCategoryByType(CategoryType type)
        {
            var categories = await _context.Categories.Where(x => x.IsActive == true && x.Type == type).OrderBy(x => x.Level == CategoryLevel.Level1).ToListAsync();
            if (categories != null)
            {
                return categories.Select(x => x.ReturnViewModel()).ToList();
            }
            return new List<CategoryVm>();
        }

    }
}
