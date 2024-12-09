using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Interfaces;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public UserRepository(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _jwtTokenHelper = new JwtTokenHelper(config);
        }

        public async Task<bool> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                user.IsActive = false;
            }
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<AppUser> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user!;
        }

        public async Task<PagedResult<AppUser>> GetUsersPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword)
                                || x.PhoneNumber.Contains(request.Keyword)
                                || x.DisplayName.Contains(request.Keyword)
                                || x.Email.Contains(request.Keyword));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<AppUser>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<bool> Update(AppUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
