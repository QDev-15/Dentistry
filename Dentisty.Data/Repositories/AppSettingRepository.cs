using Dentistry.Data.GeneratorDB.EF;
using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class AppSettingRepository : Repository<AppSetting>, IAppSettingRepository
    {
        private readonly DentistryDbContext _dbContext;
        public AppSettingRepository(DentistryDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<AppSettingVm> Update(AppSettingVm appSettingVm)
        {
            var updateAppSetting = await GetByIdAsync(appSettingVm.Id);
            if (updateAppSetting != null) {
                updateAppSetting.Hotline1 = appSettingVm.Hotline1;
                updateAppSetting.Hotline2 = appSettingVm.Hotline2;
                updateAppSetting.Hotline3 = appSettingVm.Hotline3;
                updateAppSetting.Hotline4 = appSettingVm.Hotline4;
                updateAppSetting.Facebook1 = appSettingVm.Facebook1;
                updateAppSetting.Facebook2 = appSettingVm.Facebook2;
                updateAppSetting.Facebook3 = appSettingVm.Facebook3;
                updateAppSetting.Instagram1 = appSettingVm.Instagram1;
                updateAppSetting.Instagram2 = appSettingVm.Instagram2;
                updateAppSetting.Instagram3 = appSettingVm.Instagram3;
                updateAppSetting.Twitter1 = appSettingVm.Twitter1;
                updateAppSetting.Twitter2 = appSettingVm.Twitter2;
                updateAppSetting.Twitter3 = appSettingVm.Twitter3;
                updateAppSetting.Zalo1 = appSettingVm.Zalo1;
                updateAppSetting.Zalo2 = appSettingVm.Zalo2;
                updateAppSetting.Zalo3 = appSettingVm.Zalo3;
                updateAppSetting.Zalo4 = appSettingVm.Zalo4;
                Update(updateAppSetting);
                await SaveChangesAsync();
                return updateAppSetting.ReturnViewModel();
            } else
            {
                await AddAsync(new AppSetting() { Name = "Nhiên Dentisty" });
                await SaveChangesAsync();
            }
            return appSettingVm;
        }
    }
}
