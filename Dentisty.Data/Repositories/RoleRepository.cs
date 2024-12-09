using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleRepository(RoleManager<AppRole> roleManager)
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
