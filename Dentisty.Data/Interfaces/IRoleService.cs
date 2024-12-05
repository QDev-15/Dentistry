using Dentistry.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.Data.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleVm>> GetAll();
    }
}