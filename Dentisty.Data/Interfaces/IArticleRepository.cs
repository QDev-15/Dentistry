using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace Dentisty.Data.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<Article> GetByIdAdminAsync(int id);
        Task<Article> GetByAliasAsync(string alias);
        Task<IEnumerable<Article>> GetAllAsync();
        Task<DataTableResponse<ArticleVm>> GetListAsync([FromQuery] DataTableRequest request);
        Task<ArticleVm> CreateNew(ArticleVm item);
        Task<ArticleVm> UpdateArticle(ArticleVm item);
        Task<IEnumerable<ArticleVm>> GetArticleByIds(string ids);
        Task<IEnumerable<ArticleVm>> GetForApplication(ArticleType type);
        Task<List<ArticleVm>> GetByType(ArticleType type);
        Task<PagedResult<ArticleVm>> GetForSearch(string keyWord, int pageIndex);
        Task<PagedResult<ArticleVm>> GetForCategory(int categoryId, int pageIndex);
        Task<List<ArticleVm>> GetArticleNew();
        Task<List<ArticleVm>> SiteMap();
        Task<IEnumerable<ArticleVm>> GetByCategoryId(int id);
        Task<bool> DeleteArticle(int id);
        Task<bool> DeleteArticle(Article art);
        /// <summary>
        /// If exixts return True, else return False
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        Task<bool> CheckExistsAlias(ArticleVm item);
        Task<string> GenerateAlias(ArticleVm item);

    }
}
