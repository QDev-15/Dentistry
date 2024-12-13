using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
using System.Numerics;

namespace Dentisty.Data.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<Article> GetByIdAsync(int id);
        Task<IEnumerable<Article>> GetAllAsync();
        Task<ArticleVm> CreateNew(ArticleVm item);
        Task<ArticleVm> UpdateCategory(ArticleVm item);
        /// <summary>
        /// If exixts return True, else return False
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        Task<bool> CheckExistsAlias(string alias);
    }
}
