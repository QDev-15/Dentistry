using Dentisty.Data.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Dentisty.Data.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IAppSettingRepository _appSettingRepository;
        private readonly ISmtpCredentialProtector _credentialProtector;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IAppSettingRepository appSettingRepository, ISmtpCredentialProtector credentialProtector, ILogger<EmailService> logger)
        {
            _appSettingRepository = appSettingRepository;
            _credentialProtector = credentialProtector;
            _logger = logger;
        }

        public async Task<EmailSendResult> SendAsync(string subject, string htmlBody, CancellationToken cancellationToken = default)
        {
            // Read the raw entity (not the ViewModel) - the ViewModel never carries the
            // decrypted password, by design, so it can be sent safely to the browser.
            var setting = (await _appSettingRepository.GetAllAsync()).FirstOrDefault();
            if (setting == null)
            {
                return EmailSendResult.Fail("Chưa cấu hình SMTP. Vào Cài đặt chung > Cấu hình Email thông báo để thiết lập trước.");
            }

            var password = _credentialProtector.Unprotect(setting.SmtpPasswordEncrypted);
            return await SendCoreAsync(
                setting.SmtpProvider, setting.SmtpHost, setting.SmtpPort, setting.SmtpUseSsl,
                setting.SmtpUsername, password, setting.SmtpSenderName, setting.NotificationEmails,
                subject, htmlBody, cancellationToken);
        }

        public async Task<EmailSendResult> SendToAsync(string recipientEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                return EmailSendResult.Fail("Không có địa chỉ email người nhận.");
            }

            var setting = (await _appSettingRepository.GetAllAsync()).FirstOrDefault();
            if (setting == null)
            {
                return EmailSendResult.Fail("Chưa cấu hình SMTP. Vào Cài đặt chung > Cấu hình Email thông báo để thiết lập trước.");
            }

            var password = _credentialProtector.Unprotect(setting.SmtpPasswordEncrypted);
            return await SendCoreAsync(
                setting.SmtpProvider, setting.SmtpHost, setting.SmtpPort, setting.SmtpUseSsl,
                setting.SmtpUsername, password, setting.SmtpSenderName, recipientEmail,
                subject, htmlBody, cancellationToken);
        }

        public async Task<EmailSendResult> SendTestAsync(SmtpTestSettings overrideSettings, CancellationToken cancellationToken = default)
        {
            // "Gửi thử" tests the values currently on the settings form, not what's already
            // saved - that's the whole point of testing before hitting Cập nhật. If the admin
            // left the password field blank (meaning "keep the stored one"), fall back to the
            // already-encrypted password from the database instead of failing the test.
            var password = overrideSettings.SmtpPassword;
            if (string.IsNullOrEmpty(password))
            {
                var stored = (await _appSettingRepository.GetAllAsync()).FirstOrDefault();
                password = _credentialProtector.Unprotect(stored?.SmtpPasswordEncrypted);
            }

            return await SendCoreAsync(
                overrideSettings.SmtpProvider, overrideSettings.SmtpHost, overrideSettings.SmtpPort, overrideSettings.SmtpUseSsl,
                overrideSettings.SmtpUsername, password, overrideSettings.SmtpSenderName, overrideSettings.NotificationEmails,
                "Email thử nghiệm từ hệ thống", "Đây là email thử nghiệm cấu hình SMTP từ trang quản trị.", cancellationToken);
        }

        private async Task<EmailSendResult> SendCoreAsync(
            string? provider, string? host, int? port, bool useSsl,
            string? username, string? password, string? senderName, string? notificationEmails,
            string subject, string htmlBody, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(host) || !port.HasValue || string.IsNullOrWhiteSpace(username))
            {
                return EmailSendResult.Fail("Chưa cấu hình SMTP. Vào Cài đặt chung > Cấu hình Email thông báo để thiết lập trước.");
            }
            if (string.IsNullOrEmpty(password))
            {
                return EmailSendResult.Fail("Chưa có mật khẩu SMTP hợp lệ. Vào Cài đặt chung để nhập lại mật khẩu/App Password.");
            }

            var recipients = (notificationEmails ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct()
                .ToList();
            if (recipients.Count == 0)
            {
                return EmailSendResult.Fail("Chưa cấu hình email nhận thông báo.");
            }

            var message = new MimeMessage();
            var resolvedSenderName = string.IsNullOrWhiteSpace(senderName) ? username : senderName;
            message.From.Add(new MailboxAddress(resolvedSenderName, username));
            foreach (var recipient in recipients)
            {
                message.To.Add(MailboxAddress.Parse(recipient));
            }
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            try
            {
                var socketOptions = useSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
                await client.ConnectAsync(host, port!.Value, socketOptions, cancellationToken);
                await client.AuthenticateAsync(username, password, cancellationToken);
                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);
                return EmailSendResult.Ok();
            }
            catch (AuthenticationException)
            {
                _logger.LogWarning("SMTP authentication failed for {Username}", username);
                return EmailSendResult.Fail(provider == "Gmail"
                    ? "Xác thực Gmail thất bại. Kiểm tra lại đã dùng App Password (không phải mật khẩu Gmail thường) và đã bật Xác minh 2 bước chưa."
                    : "Xác thực SMTP thất bại. Kiểm tra lại tài khoản/mật khẩu với nhà cung cấp hosting.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gửi email thất bại");
                return EmailSendResult.Fail(provider == "Hosting"
                    ? $"Không kết nối được máy chủ SMTP ({host}:{port}). Kiểm tra lại Host/Port hoặc hosting có chặn cổng gửi mail không."
                    : $"Gửi email thất bại: {ex.Message}");
            }
        }
    }
}
