using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Storages;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Common;
using Dentisty.Data.Common.Enums;
using Dentisty.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhienDentistry.Core.Catalog.Articles
{
    public class ArticlesService
    {
        private readonly IRepository<Article> _repositoryArticle;
        private readonly IRepository<Image> _repositoryImage;
        private readonly IStorageService _fileStorageService;
        public ArticlesService(IStorageService storage,
            IRepository<Article> repositoryArticle,
            IRepository<Image> repositoryImage)
        {
            _repositoryArticle = repositoryArticle;
            _fileStorageService = storage;
            _repositoryImage = repositoryImage;
        }

        public async Task<ArticleVm> Create(ArticleRequestCreated request)
        {
            try
            {
                var art = new Article()
                {
                    LanguageId = 1,
                    Title = request.Title,
                    Alias = request.Alias,
                    SortOrder = request.SortOrder,
                    Description = request.Description,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    showHome = request.showHome,
                    Images = []
                };
                if (request.ThumbnailImages.Count > 0)
                {
                    foreach (var file in request.ThumbnailImages)
                    {
                        if (file != null)
                        {
                            var _image = new Image()
                            {
                                FileSize = file.Length,
                                CreatedDate = DateTime.Now,
                                Path = await _fileStorageService.SaveFileAsync(file),
                                Type = file.ContentType,

                            };
                            art.Images.Add(_image);
                        }
                    };
                }
                await _repositoryArticle.AddAsync(art);
                await _repositoryArticle.SaveChangesAsync();

                return new ArticleVm()
                {
                    Alias = art.Alias,
                    CreatedDate = art.CreatedDate,
                    Description = art.Description,
                    Id = art.Id,
                    Name = art.Title,
                    showHome = art.showHome,
                    SortOrder = art.SortOrder,
                    UpdatedDate = art.UpdatedDate,
                };
            }
            catch (Exception ex) {
                return new ArticleVm() { };
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var article = await _repositoryArticle.GetByIdAsync(id);
                if (article != null)
                {
                    if (article.Images.Count > 0)
                    {
                        _repositoryImage.DeleteRange(article.Images);
                    }
                    article.Status = Status.InActive;
                }
                await _repositoryImage.SaveChangesAsync();
                await _repositoryArticle.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) {
                return false;   
            }
        }

        public async Task<PagedResult<ArticleVm>> GetAllPaging(GetManageArticlePagingRequest request)
        {
            var query = await _repositoryArticle.GetAllAsync();
            if (query != null && !string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Title.Contains(request.Keyword, StringComparison.CurrentCultureIgnoreCase));
            }
            if (query != null && request.LanguageId != null)
            {
                query = query.Where(x => x.LanguageId == request.LanguageId);
            }
            if (query != null && request.CategoryId != null)
            {
                query = query.Where(x => x.CategoryId == request.CategoryId);
            }

            var total = query.Count();

            query = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);

            var articles = query.Select(x => new ArticleVm() {
                Name = x.Title,
                Alias = x.Alias,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                showHome = x.showHome,
                Id = x.Id,
                UpdatedDate = x.UpdatedDate
            }).ToList();
            // Select and projection
            var pagedResult = new PagedResult<ArticleVm>()
            {
                TotalRecords = total,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = articles
            };
            return pagedResult;

        }

        public async Task<ArticleVm> GetById(int id)
        {
            var article = await _repositoryArticle.GetByIdAsync(id);
            var articleVm = new ArticleVm();
            if (article != null)
            {
                articleVm = new ArticleVm()
                {
                    Id = article.Id,
                    Name = article.Title,
                    Alias = article.Alias,
                    CreatedDate = article.CreatedDate,
                    Description = article.Description,
                    showHome = article.showHome,
                    SortOrder = article.SortOrder,
                    UpdatedDate = article.UpdatedDate
                };
            }
            return articleVm;
        }

        public Task<ArticleVm> Update(ArticleRequestUpdated request)
        {
            throw new NotImplementedException();
        }
    }
}
