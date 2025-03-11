using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.Articles;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Services
{
    public class ApplicationService
    {
        private readonly ICategoryReposiroty _cat;
        private readonly IAppSettingRepository _setting;
        private readonly ICacheService _cache;
        private readonly IBranchesRepository _branchesRepository;
        public ApplicationService(IBranchesRepository branchesRepository, ICategoryReposiroty cat, IAppSettingRepository setting, IArticleRepository article, ICacheService cache)
        {
            _cat = cat;
            _setting = setting;
            _cache = cache; 
            _branchesRepository = branchesRepository;
        }

        public async Task<ApplicationVm> GetApplication()
        {
            return await _cache.GetOrSetAsync("ApplicationWebsite", async () =>
            {
                ApplicationVm application = new ApplicationVm();
                var setting = await _setting.GetById(1);
                var categories = await _cat.GetParents();
                var branches = await _branchesRepository.GetActive();
                application.Settings = setting;
                application.Branches = branches.ToList();
                application.Categories = categories.Select(c => c.ReturnViewModel()).ToList();
                return application;
            });
        }
        public void InvalidateCache()
        {
            _cache.RemoveAsync("ApplicationWebsite"); // Xóa cache khi có cập nhật từ DB
        }
    }
}
