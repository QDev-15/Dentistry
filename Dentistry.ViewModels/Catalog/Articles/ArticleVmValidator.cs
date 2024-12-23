using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Articles
{
    public class ArticleVmValidator :  AbstractValidator<ArticleVmAddEdit>
    {
        public ArticleVmValidator() {
            RuleFor(x => x.Item.Title).NotEmpty().WithMessage("Tiêu đề không được trống.");
            RuleFor(x => x.Item.CategoryId).NotEmpty().WithMessage("Danh mục không được trống.");
        }
    }
}
