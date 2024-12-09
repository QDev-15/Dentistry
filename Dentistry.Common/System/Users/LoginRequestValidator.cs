using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.ViewModels.System.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Tài khoản là bắt buộc.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu là bắt buộc.");
        }
    }
}