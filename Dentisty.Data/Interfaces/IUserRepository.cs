using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.Data.Interfaces
{
    public interface IUserRepository
    {

        Task<bool> Update(AppUser user);

        Task<PagedResult<AppUser>> GetUsersPaging(GetUserPagingRequest request);

        Task<AppUser> GetById(Guid id);

        Task<bool> Delete(Guid id);

    }
}