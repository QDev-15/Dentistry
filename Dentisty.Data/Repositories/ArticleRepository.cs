using Dentisty.Data.Common.Enums;
using Dentisty.Data.Interfaces;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Storages;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class ArticleRepository
    {
        private readonly IRepository<Article> _repositoryArticle;
        private readonly IRepository<Logger> _repositoryLogger;
        private readonly IRepository<Image> _repositoryImage;
        public ArticleRepository(IRepository<Article> repositoryArticle,
            IRepository<Logger> repositoryLogger,
            IRepository<Image> repositoryImage)
        {
            _repositoryImage = repositoryImage;
            _repositoryLogger = repositoryLogger;
            _repositoryArticle = repositoryArticle;
        }

        public async Task<Article> Create(Article request)
        {
            await _repositoryArticle.AddAsync(request);
            await _repositoryArticle.SaveChangesAsync();
            return request;            
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var article = await _repositoryArticle.GetByIdAsync(id);
                article.IsActive = false;
                if (article.Images != null && article.Images.Count > 0)
                {
                    foreach (var image in article.Images)
                    {
                        _repositoryImage.Delete(image);
                    }
                    article.Images.Clear();
                }
                await _repositoryArticle.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<Article> GetById(int id)
        {
            var article = await _repositoryArticle.GetByIdAsync(id);
            return article!;
        }
        public async Task<IEnumerable<Article>> GetAll()
        {
            var articles = await _repositoryArticle.GetAllAsync();
            return articles;
        }

        public async Task<Article> Update(Article request)
        {
            _repositoryArticle.Update(request);
            await _repositoryArticle.SaveChangesAsync();
            return request;
        }


    }
}
