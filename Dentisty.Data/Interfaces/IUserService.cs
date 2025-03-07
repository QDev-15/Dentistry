using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.Data.Interfaces
{
    public interface IUserService
    {
        Task<Result<string>> Authencate(LoginRequest request);

        Task<Result<bool>> Register(RegisterRequest request);

        Task<Result<bool>> Update(Guid id, UserUpdateRequest request);

        Task<Result<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest request);

        Task<Result<UserVm>> GetById(Guid id);

        Task<Result<bool>> Delete(Guid id);

        Task<Result<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}