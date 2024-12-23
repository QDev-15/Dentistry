using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Doctors
{
    public class DoctorVmValidator : AbstractValidator<DoctorVm>
    {
        public DoctorVmValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được trống.");
            RuleFor(x => x.Position).NotEmpty().WithMessage("Vị trí làm việc không được trống.");
            RuleFor(x => x.Dob).NotEmpty().WithMessage("Ngày sinh không được trống.");
        }
    }
}
