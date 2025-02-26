using Dentisty.Data.Common;
using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Services.System;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Dentistry.ViewModels.System.Users;
using Dentistry.ViewModels.Common;

namespace Dentistry.Tests
{
    public class UserTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
        private readonly Mock<RoleManager<AppRole>> _roleManagerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<JwtTokenHelper> _jwtTokenHelperMock;
        private readonly UserService _userService;
        public UserTests()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null
            );

            _signInManagerMock = new Mock<SignInManager<AppUser>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                null, null, null, null
            );

            _roleManagerMock = new Mock<RoleManager<AppRole>>(
                Mock.Of<IRoleStore<AppRole>>(), null, null, null, null
            );

            _configMock = new Mock<IConfiguration>();
            _jwtTokenHelperMock = new Mock<JwtTokenHelper>(_configMock.Object);

            _userService = new UserService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _roleManagerMock.Object,
                _configMock.Object
            );
        }
        [Fact]
        public async Task Authencate_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                UserName = "admin",
                Password = "Admin@1234",
                RememberMe = true
            };

            var user = new AppUser { UserName = loginRequest.UserName };
            var roles = new List<string> { "Admin", "User" };
            string fakeToken = "mocked-jwt-token";

            _userManagerMock.Setup(u => u.FindByNameAsync(loginRequest.UserName))
                            .ReturnsAsync(user);

            _signInManagerMock.Setup(s => s.PasswordSignInAsync(user, loginRequest.Password, loginRequest.RememberMe, true))
                              .ReturnsAsync(SignInResult.Success);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                            .ReturnsAsync(roles);

            _jwtTokenHelperMock.Setup(j => j.GenerateToken(user, roles))
                               .Returns(fakeToken);

            // Act
            Result<string> result = await _userService.Authencate(loginRequest);

            // Assert
            Assert.True(result.IsSuccessed);
            Assert.Equal(fakeToken, result.data);
        }
    }
}
