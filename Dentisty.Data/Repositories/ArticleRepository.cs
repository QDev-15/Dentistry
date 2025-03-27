using Dentistry.Common;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.Enums;
using Dentisty.Common;
using Dentisty.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
            return await _context.Articles.AnyAsync(a => a.Alias == articleVm.Alias && a.Id != articleVm.Id && a.IsActive);
        }
        public async Task<Article> GetByAliasAsync(string alias)
        {
            if (string.IsNullOrEmpty(alias)) return null;

            return await _context.Articles
             .Include(x => x.CreatedBy)
             .Include(x => x.Category)
             .Include(x => x.Images)
             .FirstOrDefaultAsync(a => a.Alias == alias && a.IsActive);
        }      
        public async Task<Article> GetByIdAdminAsync(int id)
        {
            return await _context.Articles
                .Include(x=>x.CreatedBy)
                .Include(x=>x.Category)
                .Include(x=>x.Images)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.Where(x => x.IsActive == true)
                .Include(x => x.CreatedBy)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .ToListAsync();
        }
        public async Task<DataTableResponse<ArticleVm>> GetListAsync([FromQuery] DataTableRequest request)
        {
            var query = _context.Articles
            .Include(x => x.Category)
            .Include(x => x.CreatedBy)
            .AsQueryable();

            // 🔥 Tìm kiếm
            if (!string.IsNullOrEmpty(request.SearchValue))
            {
                query = query.Where(x =>
                    x.Title.Contains(request.SearchValue) ||
                    x.Category.Name.Contains(request.SearchValue) ||
                    x.CreatedBy.UserName.Contains(request.SearchValue) // Tìm theo ngày
                );
            }

            // 🔥 Sắp xếp dữ liệu dựa vào cột được chọn từ DataTables
            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                switch (request.SortColumn.ToLower())
                {
                    case "title":
                        query = request.SortDirection == "asc"
                            ? query.OrderBy(x => x.Title)
                            : query.OrderByDescending(x => x.Title);
                        break;

                    case "categoryname":
                        query = request.SortDirection == "asc"
                            ? query.OrderBy(x => x.Category.Name)
                            : query.OrderByDescending(x => x.Category.Name);
                        break;

                    case "isactive":
                        query = request.SortDirection == "asc"
                            ? query.OrderBy(x => x.IsActive)
                            : query.OrderByDescending(x => x.IsActive);
                        break;

                    case "createddate":
                        query = request.SortDirection == "asc"
                            ? query.OrderBy(x => x.CreatedDate)
                            : query.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "type":
                        query = request.SortDirection == "asc"
                            ? query.OrderBy(x => x.Type)
                            : query.OrderByDescending(x => x.Type);
                        break;

                    default:
                        query = query.OrderByDescending(x => x.CreatedDate);
                        break;
                }
            }

            int totalRecords = await query.CountAsync();

            var articles = await query
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize)
                .Select(x => x.ReturnViewModel())
                .ToListAsync();

            return new DataTableResponse<ArticleVm> ()
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = totalRecords,
                Data = articles
            };
        }



        private async Task<Article> UpdateArticleImages(Article art, string newIds)
        {
            if (string.IsNullOrEmpty(newIds))
            {
                newIds = "";
            }
            string[] arrayIds = newIds.Split(",") ?? [];
            // get list Images new
            var newImages = _context.Images.Where(x => arrayIds.Contains(x.Id.ToString())).ToList();
            // get list images real
            var imagePrimaryPaths = Utilities.ExtractImageLinks(art.Description);
            var imageNeedDeletes = new List<ImageFile>();
            if (art.Images.Any())
            {
                // images delete
                imageNeedDeletes = art.Images.Where(x => !imagePrimaryPaths.Contains(x.Path)).ToList();
                // skip images
                art.Images = art.Images.Where(x => imagePrimaryPaths.Contains(x.Path)).ToList();
            }
            // check new images
            if (newImages.Any())
            {
                var needDeletes = newImages.Where(x => !imagePrimaryPaths.Contains(x.Path)).ToList();
                imageNeedDeletes.AddRange(needDeletes);
                newImages = newImages.Where(x => imagePrimaryPaths.Contains(x.Path)).ToList();
                art.Images.AddRange(newImages);
            }
            if (imageNeedDeletes.Any())
            {
                await _imageRepository.DeleteRangeFiles(imageNeedDeletes);
                _imageRepository.DeleteRangeAsync(imageNeedDeletes);
            }
            UpdateAsync(art);
            await SaveChangesAsync();
            return art;
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
                
                //if (item.ImageFiles != null && item.ImageFiles.Any())
                //{
                //    foreach (var file in item.ImageFiles)
                //    {
                //        var image = await _imageRepository.CreateAsync(file, SystemConstants.Folder.Article);
                //        art.Images.Add(image);
                //    }
                //}
                await AddAsync(art);
                await SaveChangesAsync();
                // update Images
                art = await UpdateArticleImages(art, item.ImageIds);
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
                var art = await GetByIdAdminAsync(item.Id);
              
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
                    art = await UpdateArticleImages(art, item.ImageIds);
                }
                return art.ReturnViewModel();
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
                        art.Title += Guid.NewGuid();
                        art.Alias = art.Title.ToSlus();
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
        public async Task<bool> DeleteArticle(Article art)
        {
            try
            {
                if (art == null)
                {
                    return false;
                }
                if (art != null)
                {
                    //if (art.Images != null && art.Images.Any())
                    //{
                    //    await _imageRepository.DeleteRangeFiles(art.Images);
                    //    art.Images.Clear();
                    //}
                    if (art.IsDraft)
                    {
                        _context.Remove(art);
                        _context.SaveChanges();
                    }
                    else
                    {
                        art.IsActive = false;
                        art.Alias = art.Title.ToSlus() + "-deleted-" + Guid.NewGuid();
                        UpdateAsync(art);
                        await SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _loggerRepository.QueueLog(ex.Message);
                throw new Exception(ex.Message);
            }

        }
        public async Task<string> GenerateAlias(ArticleVm item)
        {
            var alias = item.Title.ToSlus();
            if (item.IsActive)
            {
                var art = await CheckExistsAlias(item);
                if (art)
                {
                    alias = DateTime.Now.GetTimestamp() + item.Title.ToSlus();
                }
            } else
            {
                if (item.Alias.Contains("delete"))
                {
                    return item.Alias;
                } else
                {
                    return item.Title.ToSlus() + "-deleted-" + Guid.NewGuid();
                }
            }
            return alias;
        }

        public async Task<IEnumerable<ArticleVm>> GetForApplication(ArticleType type)
        {
            var appSetting = await _context.AppSettings.FirstOrDefaultAsync();
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
            var articles = await _context.Articles.Where(x => x.Type == type && ids.Contains(x.Id.ToString()) && x.IsActive)
                .Include(x => x.Images)
                .Include(x => x.Category)
                .Select(x => x.ReturnViewModel())
                .ToListAsync();
            return articles;
            
        }

        public async Task<IEnumerable<ArticleVm>> GetByCategoryId(int id)
        {
            var articles = await _context.Articles.Where(x => x.CategoryId == id && x.IsActive)
                .Include(x => x.Images)
                .ToListAsync();
            return articles.Select(x => x.ReturnViewModel());
        }

        public async Task<PagedResult<ArticleVm>> GetForSearch(string keyWord, int pageIndex)
        {
            keyWord = keyWord ?? "";
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            int pageSize = 10;
            int skip = (pageIndex - 1) * pageSize;

            var articles = await _context.Articles.Where(x => x.Title.ToLower().Contains(keyWord.ToLower()) && x.IsActive).OrderByDescending(x => x.UpdatedDate)
                .Include(x => x.Category)
                .Include(x=> x.Images)
                .Skip(skip).Take(pageSize)
                .ToListAsync();
            var count = await _context.Articles.Where(x => x.Title.ToLower().Contains(keyWord.ToLower()) && x.IsActive).CountAsync();
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
            var articles = await _context.Articles.Where(x => x.IsActive).OrderBy(x => x.CreatedDate)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Take(10).ToListAsync();
            return articles.Select(x => x.ReturnViewModel()).ToList();
        }
        public async Task<List<ArticleVm>> SiteMap()
        {
            var arts = await _context.Articles.Where(x => x.IsActive).Select(a => new ArticleVm() { Alias = a.Alias, UpdatedDate = a.UpdatedDate }).ToListAsync();
            return arts.ToList();
        }

        public async Task<IEnumerable<ArticleVm>> GetArticleByIds(string ids)
        {
            string[] listIds = string.IsNullOrEmpty(ids) == true ? [] : ids.Split(",");

            var articles = await _context.Articles.Where(x => ids.Contains(x.Id.ToString()) && x.IsActive)
                .Include(x => x.Images)
                .Include(x => x.Category)
                .Select(x => x.ReturnViewModel()).ToListAsync();
            return articles;
        }

        public async Task<List<ArticleVm>> GetByType(ArticleType type)
        {
            var articleByTypes = await _context.Articles.Where(x => x.IsActive && x.Type == type).Include(x=>x.Images).ToListAsync();
            if (articleByTypes == null) return new List<ArticleVm>();
            return articleByTypes.Select(x => x.ReturnViewModel()).ToList();
        }

        public async Task<PagedResult<ArticleVm>> GetForCategory(int categoryId, int pageIndex)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            int pageSize = 10;
            int skip = (pageIndex - 1) * pageSize;

            var articles = await _context.Articles.Where(x => x.CategoryId == categoryId && x.IsActive).OrderByDescending(x => x.UpdatedDate)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Skip(skip).Take(pageSize).ToListAsync();
            var count = await _context.Articles.Where(x => x.CategoryId == categoryId).CountAsync();
            var result = new PagedResult<ArticleVm>()
            {
                Items = articles.Select(x => x.ReturnViewModel()).ToList(),
                PageIndex = pageIndex,
                PageSize = 10,
                TotalRecords = count
            };
            return result;
        }
    }
}
