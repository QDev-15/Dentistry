using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Slide
{
    public class SlideVmValidator : AbstractValidator<SlideVm>
    {
        public SlideVmValidator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên slide không được trống.");
        }
    }
}
