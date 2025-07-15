using Dentistry.ViewModels.Catalog.Accesss;
using Dentistry.ViewModels.Common;
using Dentisty.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface IAccessRepository
    {
        Task<PagedResult<VisitorLog>> GetVisitorLogs(PagingRequestBase request);
        Task<ActiveUserVm> CreateActiveUser(ActiveUserVm user);
        Task<ActiveUserVm> UpdateActiveUser(ActiveUserVm user);
        Task<bool> ClearVisitorLogs();
        Task<bool> ClearActiveUsers();
        Task<int> CountActiveUsers();
        Task<int> CountVistorLogs();
    }
}
