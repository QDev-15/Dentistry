using Microsoft.AspNetCore.Mvc;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentisty.Data.Interfaces;
using Dentistry.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Dentisty.Data.Services.Email;
using System.Net;

namespace Dentistry.Web.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactRepository _contactRepository;
        private readonly IBranchesRepository _branchesRepository;
        private readonly ILogger<ContactController> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ContactController(IContactRepository contactRepository, IBranchesRepository branchesRepository, ICompositeViewEngine viewEngine,
            ILogger<ContactController> logger, IServiceScopeFactory serviceScopeFactory):base(viewEngine) {
            _contactRepository = contactRepository;
            _branchesRepository = branchesRepository;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadBookForm()
        {
            return PartialView("~/Views/Contact/Partials/BookForm.cshtml", new BookFormVm());
        }

        [HttpPost]
        public async Task<IActionResult> Book(BookFormVm model)
        {
            try
            {
                if (!ModelState.IsValid || model == null)
                {
                    return PartialView("~/Views/Contact/Partials/BookForm.cshtml", model);
                }


                var contact = await _contactRepository.Create(model.contact);

                // Lấy tên cơ sở (nếu có) trước khi rời khỏi scope của request để đưa vào email
                string branchName = null;
                if (model.contact.BranchesId.HasValue && model.contact.BranchesId.Value > 0)
                {
                    var branch = await _branchesRepository.GetById(model.contact.BranchesId.Value);
                    branchName = branch?.Name;
                }
                SendBookingEmails(model.contact.Name, model.contact.PhoneNumber, model.contact.Email, model.contact.TimeBook, branchName, model.contact.Note);

                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex) {
                return Json(new ErrorResult<bool> { Message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddMessage()
        {
            return PartialView("~/Views/Contact/Partials/AddMessage.cshtml", new ContactVm());
        }
        [HttpPost]
        public async Task<IActionResult> AddMessage(ContactVm model)
        {
            if (!ModelState.IsValid)
            {
                // Render HTML từ PartialView và trả về trong JSON
                var partialViewHtml = await RenderViewToStringAsync("Partials/AddMessage", model);
                return Json(new ErrorResult<string>
                {
                    data = partialViewHtml,
                    Message = "Validation failed"
                });
            }

            try
            {
                var contact = await _contactRepository.Create(model);

                SendContactEmails(model.Name, model.PhoneNumber, model.Email, model.Message);

                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex) {
                return Json(new ErrorResult<bool> { Message = ex.Message });
            }
        }

        // Nội dung mặc định gửi khách hàng khi Admin chưa tự nhập nội dung riêng trong Cài đặt chung.
        private const string DefaultCustomerEmailTemplate =
            "Xin chào {Name},\n\nCảm ơn quý khách đã liên hệ với chúng tôi. Chúng tôi đã nhận được thông tin và sẽ phản hồi trong thời gian sớm nhất.\n\nTrân trọng!";

        // Gửi email thông báo cho nhân viên + (tuỳ chọn) email cảm ơn cho khách hàng, khi có tin
        // nhắn liên hệ mới. Chạy nền, không chặn phản hồi cho khách và không dùng lại các service
        // theo scope của request (vì scope sẽ bị dispose sau khi request kết thúc) - mỗi lần gửi
        // tạo một scope DI mới.
        private void SendContactEmails(string name, string phoneNumber, string email, string message)
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var appSettingRepository = scope.ServiceProvider.GetRequiredService<IAppSettingRepository>();

                try
                {
                    var subject = $"Có tin nhắn liên hệ mới từ {name}";
                    var htmlBody = $@"
                        <p><strong>Họ tên:</strong> {WebUtility.HtmlEncode(name)}</p>
                        <p><strong>Số điện thoại:</strong> {WebUtility.HtmlEncode(phoneNumber)}</p>
                        {(string.IsNullOrWhiteSpace(email) ? "" : $"<p><strong>Email:</strong> {WebUtility.HtmlEncode(email)}</p>")}
                        <p><strong>Nội dung tin nhắn:</strong><br/>{WebUtility.HtmlEncode(message)}</p>";

                    var result = await emailService.SendAsync(subject, htmlBody);
                    if (!result.Success)
                    {
                        _logger.LogWarning("Gửi email thông báo tin nhắn liên hệ mới thất bại: {Error}", result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Gửi email thông báo tin nhắn liên hệ mới gặp lỗi ngoài dự kiến");
                }

                await SendCustomerConfirmationEmailAsync(emailService, appSettingRepository, name, email, $"Cảm ơn {name} đã liên hệ với chúng tôi");
            });
        }

        // Gửi email thông báo cho nhân viên + (tuỳ chọn) email xác nhận cho khách hàng, khi có yêu
        // cầu đặt lịch mới. Cùng nguyên tắc chạy nền như trên.
        private void SendBookingEmails(string name, string phoneNumber, string email, DateTime? timeBook, string branchName, string note)
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var appSettingRepository = scope.ServiceProvider.GetRequiredService<IAppSettingRepository>();

                try
                {
                    var subject = $"Có yêu cầu đặt lịch mới từ {name}";
                    var htmlBody = $@"
                        <p><strong>Họ tên:</strong> {WebUtility.HtmlEncode(name)}</p>
                        <p><strong>Số điện thoại:</strong> {WebUtility.HtmlEncode(phoneNumber)}</p>
                        <p><strong>Thời gian đặt lịch:</strong> {(timeBook.HasValue ? timeBook.Value.ToString("HH:mm dd/MM/yyyy") : "Chưa xác định")}</p>
                        <p><strong>Cơ sở:</strong> {WebUtility.HtmlEncode(branchName ?? "Chưa xác định")}</p>
                        {(string.IsNullOrWhiteSpace(note) ? "" : $"<p><strong>Ghi chú:</strong><br/>{WebUtility.HtmlEncode(note)}</p>")}";

                    var result = await emailService.SendAsync(subject, htmlBody);
                    if (!result.Success)
                    {
                        _logger.LogWarning("Gửi email thông báo đặt lịch mới thất bại: {Error}", result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Gửi email thông báo đặt lịch mới gặp lỗi ngoài dự kiến");
                }

                await SendCustomerConfirmationEmailAsync(emailService, appSettingRepository, name, email, $"Xác nhận yêu cầu đặt lịch của {name}");
            });
        }

        // Gửi email cảm ơn/xác nhận cho khách hàng nếu Admin đã bật tuỳ chọn này trong Cài đặt
        // chung và khách có nhập email. Dùng chung cho cả 2 luồng liên hệ/đặt lịch.
        private async Task SendCustomerConfirmationEmailAsync(IEmailService emailService, IAppSettingRepository appSettingRepository, string name, string customerEmail, string subject)
        {
            if (string.IsNullOrWhiteSpace(customerEmail))
            {
                return;
            }

            try
            {
                var setting = await appSettingRepository.GetFirst();
                if (setting == null || !setting.SendCustomerConfirmationEmail)
                {
                    return;
                }

                var template = string.IsNullOrWhiteSpace(setting.CustomerEmailTemplate) ? DefaultCustomerEmailTemplate : setting.CustomerEmailTemplate;
                var htmlBody = WebUtility.HtmlEncode(template).Replace("\n", "<br/>").Replace("{Name}", WebUtility.HtmlEncode(name));

                var result = await emailService.SendToAsync(customerEmail, subject, htmlBody);
                if (!result.Success)
                {
                    _logger.LogWarning("Gửi email xác nhận cho khách hàng ({Email}) thất bại: {Error}", customerEmail, result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gửi email xác nhận cho khách hàng gặp lỗi ngoài dự kiến");
            }
        }

    }
}
