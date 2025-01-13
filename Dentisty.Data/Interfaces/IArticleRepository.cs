using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Enums;
using Microsoft.AspNetCore.Http;
using System.Numerics;

namespace Dentisty.Data.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<Article> GetByIdAsync(int id);
        Task<Article> GetByAliasAsync(string alias);
        Task<IEnumerable<Article>> GetAllAsync();
        Task<IEnumerable<Article>> GetByPagingAsync(ArticleVmPagingRequest request);
        Task<ArticleVm> CreateNew(ArticleVm item);
        Task<ArticleVm> UpdateArticle(ArticleVm item);
        Task<IEnumerable<ArticleVm>> GetArticleForSetting();
        Task<IEnumerable<ArticleVm>> GetNewsForSetting();
        Task<IEnumerable<ArticleVm>> GetProductForSetting();
        Task<IEnumerable<ArticleVm>> GetFeedBackForSetting();
        Task<IEnumerable<ArticleVm>> GetForApplication(ArtisleType type);
        Task<bool> DeleteArticle(int id);
        /// <summary>
        /// If exixts return True, else return False
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        Task<bool> CheckExistsAlias(ArticleVm item);
        Task<string> GenerateAlias(ArticleVm item);
        Task<bool> DeleteFile(int artId, int fileId);
        Task<bool> AddFile(int artId, IEnumerable<IFormFile> file);
    }
}
