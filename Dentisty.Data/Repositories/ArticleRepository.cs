using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Common;
using Dentisty.Data.Common;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly LoggerRepository _loggerRepository;
        private readonly IImageRepository _imageRepository;
        private readonly DentistryDbContext _context;
        public ArticleRepository(DentistryDbContext context, LoggerRepository loggerRepository, IImageRepository imageRepository) : base(context)
        {
            _imageRepository = imageRepository;
            _context = context;
            _loggerRepository = loggerRepository;
        }

        public async Task<bool> CheckExistsAlias(string alias)
        {
            return await _context.Articles.AnyAsync(a => a.Alias == alias);
        }
        public async Task<Article> GetByAliasAsync(string alias)
        {
            return await _context.Articles.Include(x => x.CreatedBy).Include(x => x.Category).Include(x => x.Images).FirstOrDefaultAsync(a => a.Alias == alias);
        }
        public async Task<Article> GetByIdAsync(int id)
        {
            return await _context.Articles.Include(x=>x.CreatedBy).Include(x=>x.Category).Include(x=>x.Images).FirstAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.Include(x => x.CreatedBy).Include(x => x.Category).Include(x => x.Images).ToListAsync();
        }

        public async Task<ArticleVm> CreateNew(ArticleVm item)
        {
            try
            {
                var art = new Article()
                {
                    Alias = await GenerateAlias(item),
                    CategoryId = item.CategoryId,
                    CreatedById = item.CreatedById,
                    CreatedDate = DateTime.Now,
                    Description = item.Description,
                    IsActive = item.IsActive,
                    Title = item.Title,
                    UpdatedDate = DateTime.Now
                };
                if (item.ImageFiles != null && item.ImageFiles.Any())
                {
                    foreach (var file in item.ImageFiles)
                    {
                        var image = await _imageRepository.CreateAsync(file);
                        art.Images.Add(image);
                    }
                }
                await AddAsync(art);
                await SaveChangesAsync();
                item.Images = art.Images.Select(art => art.ReturnViewModel()).ToList();
                return item;
            }
            catch (Exception ex) {
                _loggerRepository.QueueLog(ex.Message, "Create new Article");
                return null;
            }

        }


        public async Task<ArticleVm> UpdateCategory(ArticleVm item)
        {
            try
            {
                var art = await GetByIdAsync(item.CategoryId);
                if (art != null)
                {
                    art.CategoryId = item.CategoryId;
                    art.Title = item.Title;
                    art.Alias = await GenerateAlias(item);
                    art.Description = item.Description;
                    art.IsActive = item.IsActive;
                    art.UpdatedDate = DateTime.Now;
                    art.CreatedById = new Guid(_loggerRepository.GetCurrentUserId());
                    Update(art);
                    await SaveChangesAsync();
                }
                return item;
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message);
                return item;
            }

        }

        public async Task<string> GenerateAlias(ArticleVm item)
        {
            var alias = item.Title.ToSlus();
            var art = await CheckExistsAlias(alias);
            if (art)
            {
                alias = DateTime.Now.GetTimestamp() + item.Title.ToSlus();
            }
            return alias;
        }

        public async Task<bool> DeleteFile(int artId, int fileId)
        {
            try
            {
                var art = await GetByIdAsync(artId);
                if (art != null)
                {
                    if (art.Images != null && art.Images.Any())
                    {
                        var imageDel = art.Images.FirstOrDefault(x=>x.Id == fileId);
                        if (imageDel != null)
                        {
                            await _imageRepository.DeleteFile(imageDel);
                            art.Images.Remove(imageDel);
                        }
                    }
                    Update(art);
                    await SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog($"{ex.Message}", "Delete file width artId: " + artId + " fileId: " + fileId);
                return false;
            }
        }

        public async Task<bool> AddFile(int artId, IEnumerable<IFormFile> files)
        {
            try
            {
                var art = await GetByIdAsync(artId);
                if (art != null)
                {
                    if (files != null && files.Any())
                    {
                        foreach (var file in files)
                        {
                            var image = await _imageRepository.CreateAsync(file);
                            art.Images.Add(image);
                        }
                    }
                    Update(art);
                    await SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog($"{ex.Message}", "Delete file width artId: " + artId);
                return false;
            }
        }

        public async Task<IEnumerable<Article>> GetByPagingAsync(ArticleVmPagingRequest request)
        {
            IQueryable<Article> query = _context.Articles;

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => EF.Functions.Like(x.Alias, $"%{request.Keyword}%"));
            }

            // paging
            if (request.PageIndex > 0 && request.PageSize > 0)
            {
                query = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
            }

            return await query.ToListAsync();
        }
    }
}
