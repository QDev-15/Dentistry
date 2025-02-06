using FluentValidation;

namespace Dentistry.ViewModels.Catalog.Tags
{
    public class TagsVmValidator : AbstractValidator<TagsVm>
    {
        public TagsVmValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được trống.");
        }
    }
}
