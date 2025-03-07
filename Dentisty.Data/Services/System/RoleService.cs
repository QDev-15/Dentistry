using Dentistry.Data.GeneratorDB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dentisty.Data.Services.System
{
    public class RoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<AppRole>> GetAll()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return roles;
        }

    }
}