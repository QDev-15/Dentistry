using Dentistry.Data.GeneratorDB.EF;
using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services.Email;
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
        private readonly ISmtpCredentialProtector _smtpCredentialProtector;
        public AppSettingRepository(DentistryDbContext context, ISmtpCredentialProtector smtpCredentialProtector) : base(context)
        {
            _dbContext = context;
            _smtpCredentialProtector = smtpCredentialProtector;
        }

        public async Task<AppSettingVm> GetById(int id)
        {
            if (id == 0) {
                return (await _dbContext.AppSettings.FirstOrDefaultAsync()).ReturnViewModel();
            }
            return (await _dbContext.AppSettings.FirstOrDefaultAsync(x => x.Id == id)).ReturnViewModel();
        }

        public async Task<AppSettingVm> GetFirst()
        {
            return (await _dbContext.AppSettings.FirstOrDefaultAsync()).ReturnViewModel();
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

                updateAppSetting.Name = appSettingVm.Name;
                updateAppSetting.Description = appSettingVm.Description;
                updateAppSetting.HomeImageUrl = appSettingVm.HomeImageUrl;

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
                updateAppSetting.DoctorListTitle = appSettingVm.DoctorListTitle;
                updateAppSetting.DoctorListSubTitle = appSettingVm.DoctorListSubTitle;
                updateAppSetting.FeedbackListTitle = appSettingVm.FeedbackListTitle;

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

                updateAppSetting.SmtpProvider = appSettingVm.SmtpProvider;
                updateAppSetting.SmtpHost = appSettingVm.SmtpHost;
                updateAppSetting.SmtpPort = appSettingVm.SmtpPort;
                updateAppSetting.SmtpUseSsl = appSettingVm.SmtpUseSsl;
                updateAppSetting.SmtpUsername = appSettingVm.SmtpUsername;
                updateAppSetting.SmtpSenderName = appSettingVm.SmtpSenderName;
                updateAppSetting.NotificationEmails = appSettingVm.NotificationEmails;
                updateAppSetting.SendCustomerConfirmationEmail = appSettingVm.SendCustomerConfirmationEmail;
                updateAppSetting.CustomerEmailTemplate = appSettingVm.CustomerEmailTemplate;
                // Only re-encrypt & overwrite when the admin actually typed a new password;
                // an empty field on submit means "keep the currently stored one".
                if (!string.IsNullOrEmpty(appSettingVm.SmtpPassword))
                {
                    updateAppSetting.SmtpPasswordEncrypted = _smtpCredentialProtector.Protect(appSettingVm.SmtpPassword);
                }

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

        public async Task UpdateLocationTracking(int id, bool value)
        {
            try
            {
                var updateAppSetting = await GetByIdAsync(id);
                updateAppSetting.TrackVisitorLocation = value;
                await SaveChangesAsync();
            } catch(Exception ex)
            {

            }
        }
    }
}
