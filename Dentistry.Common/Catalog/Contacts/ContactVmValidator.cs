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
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được trống.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Số điện thoại không được trống.");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Nội dung không được trống.");
        }
    }
}
