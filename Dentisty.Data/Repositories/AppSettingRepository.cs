using Dentistry.Data.GeneratorDB.EF;
using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Dentistry.Common.SystemConstants;

namespace Dentisty.Data.Repositories
{
    public class AppSettingRepository : Repository<AppSetting>, IAppSettingRepository
    {
        private readonly DentistryDbContext _dbContext;
        public AppSettingRepository(DentistryDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<AppSettingVm> GetById(int id)
        {
            return (await _dbContext.AppSettings.FirstOrDefaultAsync(x => x.Id == id)).ReturnViewModel();
        }

        public async Task<AppSettingVm> Update(AppSettingVm appSettingVm)
        {
            var updateAppSetting = await GetByIdAsync(appSettingVm.Id);
            if (updateAppSetting != null) {
                updateAppSetting.ShowCategoryList = appSettingVm.ShowCategoryList;
                updateAppSetting.ShowProductList = appSettingVm.ShowProductList;
                updateAppSetting.ShowDoctorSlideList = appSettingVm.ShowDoctorSlideList;
                updateAppSetting.ShowContactList = appSettingVm.ShowContactList;
                updateAppSetting.ShowArtileSlideList = appSettingVm.ShowArtileSlideList;
                updateAppSetting.ShowFeedbackList = appSettingVm.ShowFeedbackList;
                updateAppSetting.ShowNewsList = appSettingVm.ShowNewsList;
                updateAppSetting.ShowCategoryProductList = appSettingVm.ShowCategoryProductList;

                updateAppSetting.CompanyName = appSettingVm.CompanyName;
                updateAppSetting.CompanyAddress = appSettingVm.CompanyAddress;
                updateAppSetting.CompanyEmail = appSettingVm.CompanyEmail;
                updateAppSetting.CompanyPhone = appSettingVm.CompanyPhone;
                updateAppSetting.CompanyWebsite = appSettingVm.CompanyWebsite;
                updateAppSetting.CompanyTitle = appSettingVm.CompanyTitle;
                updateAppSetting.BranchesTitle = appSettingVm.BranchesTitle;

                updateAppSetting.NewsListTitle = appSettingVm.NewsListTitle;
                updateAppSetting.CategoryListTitle = appSettingVm.CategoryListTitle;
                updateAppSetting.CategoryListSubTitle = appSettingVm.CategoryListSubTitle;
                updateAppSetting.CategoryListProductTitle = appSettingVm.CategoryListProductTitle;
                updateAppSetting.CategoryListProductSubTitle = appSettingVm.CategoryListProductSubTitle;
                updateAppSetting.DoctorListTitle = appSettingVm.DoctorListSubTitle;

                updateAppSetting.Categories = string.Join(",", appSettingVm.Categories);
                updateAppSetting.CategoryProducts = string.Join(",", appSettingVm.CategoryProducts);
                updateAppSetting.Products = string.Join(",", appSettingVm.Products);
                updateAppSetting.Doctors = string.Join(",", appSettingVm.Doctors);
                updateAppSetting.Articles = string.Join(",", appSettingVm.Articles);
                updateAppSetting.News = string.Join(",", appSettingVm.News);
                updateAppSetting.Feedbacks = string.Join(",", appSettingVm.Feedbacks);

                updateAppSetting.Facebook  = appSettingVm.Facebook;
                updateAppSetting.Instagram = appSettingVm.Instagram;
                updateAppSetting.Twitter = appSettingVm.Twitter;
                updateAppSetting.ZaloHotline = appSettingVm.ZaloHotline;
                updateAppSetting.HotlineHaNoi = appSettingVm.HotlineHaNoi;
                updateAppSetting.Youtube = appSettingVm.Youtube;
                updateAppSetting.Tiktok = appSettingVm.Tiktok;
                updateAppSetting.StartWork = appSettingVm.StartWork;
                updateAppSetting.EndWork = appSettingVm.EndWork;
                UpdateAsync(updateAppSetting);
                await SaveChangesAsync();
                return updateAppSetting.ReturnViewModel();
            }
            return appSettingVm;
        }

        public async Task UpdateAssess(int id, bool value)
        {
            try
            {
                var updateAppSetting = await GetByIdAsync(id);
                updateAppSetting.TrackVisitors = value;
                await SaveChangesAsync();
            } catch(Exception ex)
            {
                
            }
            
        }
    }
}
