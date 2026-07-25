using Dentistry.Data.GeneratorDB.EF;
using Dentistry.ViewModels.Catalog.Accesss;
using Dentistry.ViewModels.Common;
using Dentisty.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class AccessRepository : IAccessRepository
    {
        private readonly DentistryDbContext _context;
        private readonly ITimezoneService _timezone;

        public AccessRepository(DentistryDbContext context, ITimezoneService timezone)
        {
            _context = context;
            _timezone = timezone;
        }

        public async Task<bool> ClearActiveUsers()
        {
            try
            {
                await _context.ActiveUsers.ExecuteDeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ClearVisitorLogs()
        {
            try
            {
                await _context.VisitorLogs.ExecuteDeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<int> CountActiveUsers()
        {
            var count = await _context.ActiveUsers.CountAsync();
            return count;
        }

        public async Task<int> CountVistorLogs()
        {
            var count = await _context.VisitorLogs.CountAsync();
            return count;
        }

        public async Task<ActiveUserVm> CreateActiveUser(ActiveUserVm user)
        {
            var newUser = new ActiveUser
            {
                VisitorId = user.VisitorId,
                UserAgent = user.UserAgent,
                IpAddress = user.IpAddress ?? "unknow",
                LastActive = user.LastActive ?? DateTime.UtcNow,
                Latitude = user.Latitude,
                Longitude = user.Longitude,
                IsOnline = user.IsOnline ?? false
            };
            await _context.ActiveUsers.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser.ReturnViewModel();
        }

        public async Task<PagedResult<VisitorLog>> GetVisitorLogs(PagingRequestBase request)
        {
            if (request.PageIndex < 1)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize < 10)
            {
                request.PageSize = 10;
            }
            
            PagedResult<VisitorLog> result = new PagedResult<VisitorLog>();
            result.PageSize = request.PageSize;
            result.PageIndex = request.PageIndex;
            result.TotalRecords = await _context.VisitorLogs.CountAsync();
            result.OnlineUsers = await _context.ActiveUsers.Where(x => x.IsOnline).CountAsync();
            result.Items = await _context.VisitorLogs.OrderByDescending(x => x.VisitTime).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            return result;
        }

        public async Task<ActiveUserVm> UpdateActiveUser(ActiveUserVm user)
        {
            var updatedUser = await _context.ActiveUsers.FirstOrDefaultAsync(x => x.VisitorId == user.VisitorId);
            if (updatedUser != null)
            {
                if(user.LastActive != null)
                {
                    updatedUser.LastActive = user.LastActive.Value;
                }
                if(user.Latitude != null)
                {
                    updatedUser.Latitude = user.Latitude;
                }
                if(user.Longitude != null)
                {
                    updatedUser.Longitude = user.Longitude;
                }
                if(user.IsOnline != null)
                {
                    updatedUser.IsOnline = user.IsOnline.Value;
                }
                if(user.VisitorId != null)
                {
                    updatedUser.VisitorId = user.VisitorId;
                }
                if(user.UserAgent != null)
                {
                    updatedUser.UserAgent = user.UserAgent;
                }
                if(user.IpAddress != null)
                {
                    updatedUser.IpAddress = user.IpAddress;
                }
                _context.ActiveUsers.Update(updatedUser);
                await _context.SaveChangesAsync();
                return updatedUser.ReturnViewModel();
            }
            else
            {
                // If user not found, create a new one
                return await CreateActiveUser(user);
            }
        }
    }
}
