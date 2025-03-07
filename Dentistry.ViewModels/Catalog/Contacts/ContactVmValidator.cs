using Dentistry.ViewModels.Catalog.Contacts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Contacts
{
    public class ContactVmValidator : AbstractValidator<ContactVm>
    {
        public ContactVmValidator()
        {
            // Name is required
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tên không được trống.")
                .WithMessage("Tên chỉ được chứa chữ cái và khoảng trắng.");

            // PhoneNumber is required and must match a valid pattern
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Số điện thoại không được trống.")
                .Matches(@"^\+?[0-9]{1,4}?[\s.-]?[0-9]{3,4}[\s.-]?[0-9]{3,4}[\s.-]?[0-9]{0,4}$")
                .WithMessage("Số điện thoại không đúng định dạng hợp lệ.");

            // Email is optional but must be in valid email format
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email không đúng định dạng.");

            // Message is required
            RuleFor(x => x.Message)
                .NotEmpty()
                .WithMessage("Nội dung không được trống.")
                .MinimumLength(10)
                .WithMessage("Nội dung phải có ít nhất 10 ký tự.");
        }
    }
}
