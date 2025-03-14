using Dentistry.Data.GeneratorDB.EF;
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
            result.OnlineUsers = await _context.ActiveUsers.CountAsync();
            result.Items = await _context.VisitorLogs.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            return result;
        }
    }
}
