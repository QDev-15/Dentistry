using Azure.Core;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Storages;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data.Common.Enums;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services;
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
        private readonly ArticleRepository _articleRepository;
        private readonly ImageRepository _imageRepository;
        private readonly LoggerRepository logger;
        private readonly IStorageService _storageService;
        public ArticlesService(ArticleRepository articleRepository,
            ImageRepository imageRepository,
            LoggerRepository loggerRepository,
            IStorageService storageService)
        {
            _imageRepository = imageRepository;
            _articleRepository = articleRepository;
            _storageService = storageService;
            logger = loggerRepository;
        }

        public async Task<ArticleVm> Create(ArticleRequestCreated request)
        {
            try
            {
                var art = new Article()
                {
                    Title = request.Title,
                    Alias = request.Alias,
                    Description = request.Description,
                    CreatedById = new Guid(request.CreatedById),
                    CategoryId = request.CategoryId,   
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsActive = true,
                    Images = []
                };
                if (request.ThumbnailImages.Count > 0)
                {
                    foreach (var file in request.ThumbnailImages)
                    {
                        if (file != null)
                        {
                            var img = await _imageRepository.Create(file);
                            art.Images.Add(img);
                        }
                    };
                }
                var result = await _articleRepository.Create(art);

                return new ArticleVm()
                {
                    Alias = result.Alias,
                    Description = result.Description,
                    Id = result.Id,
                    Name = result.Title,
                    Images = result.Images.Select(x => new ImageVm ()
                    {
                       Id = x.Id,
                       FileSize = x.FileSize,
                       FileName = x.FileName,
                       Path = x.Path,
                       Type = x.Type
                    }).ToList(),
                    CreatedBy = new UserVm
                    {
                       Id = result.CreatedBy.Id.ToString(),
                       DisplayName = result.CreatedBy.DisplayName,
                       Dob = art.CreatedBy.Dob,
                       Email = result.CreatedBy.Email!,
                       FirstName = result.CreatedBy.FirstName,
                       LastName = result.CreatedBy.LastName,   
                       PhoneNumber = result.CreatedBy.PhoneNumber!,
                       UserName = result.CreatedBy.UserName!
                    },
                    UpdatedDate = result.UpdatedDate,
                    CreatedDate = result.CreatedDate,
                };
            }
            catch (Exception ex) {
                logger.Add(ex.Message, string.Empty, request.CreatedById);
                return new ArticleVm() { };
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var article = await _articleRepository.GetById(id);
                if (article != null)
                {
                    if (article.Images.Count > 0)
                    {
                       await _imageRepository.DeleteRange(article.Images);
                    }
                    article.IsActive = false;
                    await _articleRepository.Update(article);
                }
                
                return true;
            }
            catch (Exception ex) {
                logger.Add(ex.Message);
                return false;   
            }
        }

        public async Task<PagedResult<ArticleVm>> GetAllPaging(GetManageArticlePagingRequest request)
        {
            var query = await _articleRepository.GetAll();
            if (query != null && !string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Title.Contains(request.Keyword, StringComparison.CurrentCultureIgnoreCase));
            }
            if (query != null && request.CategoryId != null)
            {
                query = query.Where(x => x.CategoryId == request.CategoryId);
            }

            int total = 0;
            if (query != null && query.Count() > 0)
            {
                total = query.Count();
            }
            

            query = query?.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);

            var articles = query?.Select(x => new ArticleVm() {
                Name = x.Title,
                Alias = x.Alias,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                Id = x.Id,
                UpdatedDate = x.UpdatedDate
            }).ToList();
            // Select and projection
            var pagedResult = new PagedResult<ArticleVm>()
            {
                TotalRecords = total,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = articles!
            };
            return pagedResult;

        }

        public async Task<ArticleVm> GetById(int id)
        {
            var article = await _articleRepository.GetById(id);
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
