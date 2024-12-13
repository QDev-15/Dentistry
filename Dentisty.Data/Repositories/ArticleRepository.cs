using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
using Dentisty.Data.Interfaces;
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
        private readonly DentistryDbContext _context;
        public ArticleRepository(DentistryDbContext context, LoggerRepository loggerRepository) : base(context)
        {
            _context = context;
            _loggerRepository = loggerRepository;
        }

        public async Task<bool> CheckExistsAlias(string alias)
        {
            return await _context.Articles.AnyAsync(a => a.Alias == alias);
        }
        public async Task<Article> GetByIdAsync(int id)
        {
            return await _context.Articles.Include(x=>x.CreatedBy).Include(x=>x.Category).Include(x=>x.Images).FirstAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.Include(x => x.CreatedBy).Include(x => x.Category).Include(x => x.Images).ToListAsync();
        }

        public Task<ArticleVm> CreateNew(ArticleVm item)
        {
            throw new NotImplementedException();
        }


        public Task<ArticleVm> UpdateCategory(ArticleVm item)
        {
            throw new NotImplementedException();
        }
    }
}
