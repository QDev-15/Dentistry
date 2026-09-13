using FluentValidation;
using System;
using System.Linq;
using System.Net.Mail;

namespace Dentistry.ViewModels.Catalog.AppSettings
{
    public class AppSettingDataVmValidator : AbstractValidator<AppSettingDataVm>
    {
        public AppSettingDataVmValidator()
        {
            // NotificationEmails is a comma-separated list; every address in it must be valid,
            // otherwise employee notification emails will silently fail to send once a customer
            // actually contacts/books (the failure previously only surfaced in server logs).
            RuleFor(x => x.Setting.NotificationEmails)
                .Must(BeValidEmailList)
                .WithMessage("Email nhận thông báo có địa chỉ không đúng định dạng. Nhập nhiều địa chỉ cách nhau bằng dấu phẩy.")
                .When(x => x.Setting != null && !string.IsNullOrWhiteSpace(x.Setting.NotificationEmails));

            RuleFor(x => x.Setting.CustomerEmailTemplate)
                .NotEmpty()
                .WithMessage("Nội dung email gửi khách hàng không được trống khi đã bật \"Gửi email xác nhận cho khách hàng\".")
                .When(x => x.Setting != null && x.Setting.SendCustomerConfirmationEmail);
        }

        private static bool BeValidEmailList(string value)
        {
            var addresses = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return addresses.Length > 0 && addresses.All(IsValidEmail);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                return new MailAddress(email).Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
