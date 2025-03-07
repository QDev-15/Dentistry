using Dentistry.Common.Constants;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.Enums;
using Dentisty.Data.Common;
using Dentisty.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
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

        public async Task<bool> CheckExistsAlias(ArticleVm articleVm)
        {
            return await _context.Articles.AnyAsync(a => a.Alias == articleVm.Alias && a.Id != articleVm.Id);
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
            return await _context.Articles.Where(x => x.IsActive == true).Include(x => x.CreatedBy).Include(x => x.Category).Include(x => x.Images).ToListAsync();
        }

        public async Task<ArticleVm> CreateNew(ArticleVm item)
        {
            try
            {
                var art = new Article()
                {
                    Alias = string.IsNullOrEmpty(item.Alias) ? await GenerateAlias(item) : item.Alias,
                    CategoryId = item.CategoryId,
                    CreatedById = _loggerRepository.GetCurrentUserGuidId(),
                    Description = item.Description,
                    Type = item.Type,
                    IsActive = true,
                    IsDraft = item.IsDraft,
                    Title = item.Title,
                    Tags = item.Tags,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                
                if (item.ImageFiles != null && item.ImageFiles.Any())
                {
                    foreach (var file in item.ImageFiles)
                    {
                        var image = await _imageRepository.CreateAsync(file, SystemConstants.Folder.Article);
                        art.Images.Add(image);
                    }
                }
                await AddAsync(art);
                await SaveChangesAsync();
                return art.ReturnViewModel();
            }
            catch (Exception ex) {
                _loggerRepository.QueueLog(ex.Message, "Create new Article");
                throw new Exception(ex.Message);
            }

        }


        public async Task<ArticleVm> UpdateArticle(ArticleVm item)
        {
            try
            {
                var art = await GetByIdAsync(item.Id);
              
                if (art != null)
                {
                    art.IsDraft = false;
                    art.CategoryId = item.CategoryId;
                    art.Title = item.Title;
                    art.Type = item.Type;
                    art.Alias = await GenerateAlias(item);
                    art.Description = item.Description;
                    art.IsActive = item.IsActive;
                    art.UpdatedDate = DateTime.Now;
                    art.CreatedById = new Guid(_loggerRepository.GetCurrentUserId());
                    art.Tags = item.Tags;
                    
                    UpdateAsync(art);
                    await SaveChangesAsync();
                }
                return item;
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> DeleteArticle(int id)
        {
            try
            {
                var art = _context.Articles.Include(x=>x.Images).FirstOrDefault(x => x.Id == id);
                if (art != null)
                {
                    if (art.Images != null && art.Images.Any())
                    {
                        await _imageRepository.DeleteRangeFiles(art.Images);
                        art.Images.Clear();
                    }
                    if (art.IsDraft)
                    {
                        _context.Remove(art);
                        _context.SaveChanges();
                    } else
                    {
                        art.IsActive = false;
                        UpdateAsync(art);
                        await SaveChangesAsync();
                    }
                }
                return true;
            } catch(Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<string> GenerateAlias(ArticleVm item)
        {
            var alias = item.Title.ToSlus();
            var art = await CheckExistsAlias(item);
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
                    UpdateAsync(art);
                    await SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog($"{ex.Message}", "Delete file width artId: " + artId + " fileId: " + fileId);
                throw new Exception(ex.Message);
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
                            var image = await _imageRepository.CreateAsync(file, SystemConstants.Folder.Article);
                            art.Images.Add(image);
                        }
                    }
                    UpdateAsync(art);
                    await SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog($"{ex.Message}", "Delete file width artId: " + artId);
                throw new Exception(ex.Message);
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

        public async Task<IEnumerable<ArticleVm>> GetArticleForSetting()
        {
            return await _context.Articles.Where(x => x.Type == ArticleType.Article && x.IsActive).Include(x => x.CreatedBy).Select(x => x.ReturnViewModel()).ToListAsync();
        }

        public async Task<IEnumerable<ArticleVm>> GetNewsForSetting()
        {
            return await _context.Articles.Where(x => x.Type == ArticleType.News && x.IsActive).Include(x => x.CreatedBy).Select(x => x.ReturnViewModel()).ToListAsync();
        }

        public async Task<IEnumerable<ArticleVm>> GetProductForSetting()
        {
            return await _context.Articles.Where(x => x.Type == ArticleType.Products && x.IsActive).Include(x => x.CreatedBy).Select(x => x.ReturnViewModel()).ToListAsync();
        }

        public async Task<IEnumerable<ArticleVm>> GetFeedBackForSetting()
        {
            return await _context.Articles.Where(x => x.Type == ArticleType.FeedBack && x.IsActive).Include(x => x.CreatedBy).Select(x => x.ReturnViewModel()).ToListAsync();
        }

        public async Task<IEnumerable<ArticleVm>> GetForApplication(ArticleType type)
        {
            var appSetting = await _context.AppSettings.FirstOrDefaultAsync(x => x.Id == 1);
            string[] ids = [];
            if (appSetting ==null || string.IsNullOrEmpty(appSetting.Articles))
            {
                return new List<ArticleVm>();
            }
            switch (type)
            {
                case ArticleType.Article:
                    ids = appSetting.Articles!.Split(',');
                    break;
                case ArticleType.News:
                    ids = appSetting.News!.Split(',');
                    break;
                case ArticleType.FeedBack:
                    ids = appSetting.Feedbacks!.Split(',');
                    break;
                case ArticleType.Products:
                    ids = appSetting.Products!.Split(',');
                    break;
                default:
                    ids = [];
                    break;
            }
            var articles = await _context.Articles.Where(x => x.Type == type && ids.Contains(x.Id.ToString()) && x.IsActive).Include(x => x.Images).Include(x => x.Category).Select(x => x.ReturnViewModel()).ToListAsync();
            return articles;
            
        }

        public async Task<IEnumerable<ArticleVm>> GetByCategoryId(int id)
        {
            var articles = await _context.Articles.Where(x => x.CategoryId == id).Include(x => x.Images).ToListAsync();
            return articles.Select(x => x.ReturnViewModel());
        }

        public async Task<PagedResult<ArticleVm>> GetForSearch(string keyWord, int pageIndex)
        {
            keyWord = keyWord ?? "";
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            int pageSize = 10;
            int skip = (pageIndex - 1) * pageSize;

            var articles = await _context.Articles.Where(x => x.Title.ToLower().Contains(keyWord.ToLower())).Include(x => x.Category).Include(x=> x.Images).Skip(skip).Take(pageSize).ToListAsync();
            var count = await _context.Articles.Where(x => x.Title.ToLower().Contains(keyWord.ToLower())).CountAsync();
            var result = new PagedResult<ArticleVm>()
            {
                Items = articles.Select(x => x.ReturnViewModel()).ToList(),
                PageIndex = pageIndex,
                PageSize = 10,
                TotalRecords = count
            };
            return result;
        }  
        public async Task<List<ArticleVm>> GetArticleNew()
        {
            var articles = await _context.Articles.OrderBy(x => x.CreatedDate).Include(x => x.Category).Include(x => x.Images).Take(10).ToListAsync();
            return articles.Select(x => x.ReturnViewModel()).ToList();
        }
        public async Task<List<ArticleVm>> SiteMap()
        {
            var arts = await _context.Articles.Select(a => new ArticleVm() { Alias = a.Alias, UpdatedDate = a.UpdatedDate }).ToListAsync();
            return arts.ToList();
        }
    }
}
