using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Tên là bắt buộc.")
                .MaximumLength(200).WithMessage("Tên không vượt quá 200 ký tự.");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Tên họ là bắt buộc.")
                .MaximumLength(200).WithMessage("Tên họ không vượt quá 200 ký tự.");

            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Ngày sinh không vượt quá 100 năm.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email là bắt buộc.")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email không đúng.");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Số điện thoại là bắt buộc.");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Tên tài khoản là bắt buộc.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu là bắt buộc.")
                .MinimumLength(6).WithMessage("Mật khẩu ít nhất có 6 ký tự.");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Nhập lại mật khẩu không đúng.");
                }
            });
        }
    }
}