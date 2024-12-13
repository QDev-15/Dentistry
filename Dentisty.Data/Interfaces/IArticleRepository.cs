using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
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
        Task<ArticleVm> UpdateCategory(ArticleVm item);
        /// <summary>
        /// If exixts return True, else return False
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        Task<bool> CheckExistsAlias(string alias);
        Task<string> GenerateAlias(ArticleVm item);
        Task<bool> DeleteFile(int artId, int fileId);
        Task<bool> AddFile(int artId, IEnumerable<IFormFile> file);
    }
}
