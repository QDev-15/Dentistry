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
                    CreatedById = request.CreatedById,
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
                            var _image = new Image()
                            {
                                FileSize = file.Length,
                                CreatedDate = DateTime.Now,
                                Path = await _storageService.SaveFileAsync(file),
                                Type = file.ContentType,

                            };
                            art.Images.Add(_image);
                        }
                    };
                }
                var article = await _articleRepository.Create(art);

                return new ArticleVm()
                {
                    Alias = art.Alias,
                    Description = art.Description,
                    Id = art.Id,
                    Name = art.Title,
                    Images = art.Images.Select(x => new ImageVm ()
                    {
                       Id = x.Id,
                       FileSize = x.FileSize,
                       Path = x.Path,
                       Type = x.Type
                    }).ToList(),
                    CreatedBy = new UserVm
                    {
                       Id = art.CreatedBy.Id.ToString(),
                       DisplayName = art.CreatedBy.DisplayName,
                       Dob = art.CreatedBy.Dob,
                       Email = art.CreatedBy.Email,
                       FirstName = art.CreatedBy.FirstName,
                       LastName = art.CreatedBy.LastName,   
                       PhoneNumber = art.CreatedBy.PhoneNumber,
                       UserName = art.CreatedBy.UserName
                    },
                    UpdatedDate = art.UpdatedDate,
                    CreatedDate = art.CreatedDate,
                };
            }
            catch (Exception ex) {
                logger.Add(ex.Message, null, request.CreatedById);
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
                }
                await _articleRepository.Update(article);
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

            var total = query.Count();

            query = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);

            var articles = query.Select(x => new ArticleVm() {
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
                Items = articles
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
