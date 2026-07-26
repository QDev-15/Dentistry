using Dentistry.ViewModels.System.Users;
using Dentisty.Data.Services.System;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly UserService _userService;

        public ProfileController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var result = await _userService.GetById(userId.Value);
            if (!result.IsSuccessed)
            {
                return RedirectToAction("Error", "Home");
            }

            var user = result.ResultObj;
            var updateRequest = new UserUpdateRequest()
            {
                Id = userId.Value,
                Dob = user.Dob,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(updateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserUpdateRequest request)
        {
            var userId = GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Error", "Home");
            }
            // Luôn cập nhật đúng tài khoản đang đăng nhập, không tin id gửi lên từ client
            request.Id = userId.Value;

            if (!ModelState.IsValid)
                return View(request);

            var result = await _userService.Update(userId.Value, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật thông tin thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
    }
}
