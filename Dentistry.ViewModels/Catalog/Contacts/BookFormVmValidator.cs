using Dentistry.ViewModels.Catalog.Contacts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Contacts
{
    public class BookFormVmValidator : AbstractValidator<BookFormVm>
    {
        public BookFormVmValidator()
        {
            // Name is required
            RuleFor(x => x.contact.Name)
                .NotEmpty()
                .WithMessage("Tên không được trống.")
                .Matches(@"^[A-Za-zÀ-Ỵà-ỵ](?=(?:.*[A-Za-zÀ-Ỵà-ỵ]){1,})[A-Za-zÀ-Ỵà-ỵ\s]*$")
                .WithMessage("Tên ít nhất 2 chữ cái, bắt đầu bằng chữ cái và chỉ được chứa chữ cái và khoảng trắng.");

            // PhoneNumber is required and must match a valid pattern
            RuleFor(x => x.contact.PhoneNumber)
                .NotEmpty()
                .WithMessage("Số điện thoại không được trống.")
                .Matches(@"^\+?[0-9]{1,4}?[\s.-]?[0-9]{3,4}[\s.-]?[0-9]{3,4}[\s.-]?[0-9]{0,4}$")
                .WithMessage("Số điện thoại không đúng định dạng hợp lệ.");

            // Email is optional but must be in valid email format
            RuleFor(x => x.contact.Email)
                .EmailAddress()
                .WithMessage("Email không đúng định dạng.");

            // TimeBook is required
            RuleFor(x => x.contact.TimeBook)
                .NotEmpty()
                .WithMessage("Thời gian đặt lịch không được trống.");
            // Branches is required
            RuleFor(x => x.contact.BranchesId)
                .Must(x => x != null && x.Value != null && x.Value > 0)
                .WithMessage("Cơ sở khám không được trống.");
        }
    }
}
