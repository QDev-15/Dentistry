﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Categories
{
    public class CategoryVmValidator :  AbstractValidator<CategoryVm>
    {
        public CategoryVmValidator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được trống.");
            RuleFor(x => x.Alias).NotEmpty().WithMessage("Tên rút gọn không được trống");
        }
    }
}