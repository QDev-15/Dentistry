using FluentValidation;

namespace Dentistry.ViewModels.Catalog.Branches
{
    public class BranchesVmValidator :  AbstractValidator<BranchesVm>
    {
        public BranchesVmValidator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên cơ sở không được trống.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Số diện thoại không được trống.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Địa chỉ không được trống.");
        }
    }
}
