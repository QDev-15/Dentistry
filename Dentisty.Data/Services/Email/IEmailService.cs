namespace Dentisty.Data.Services.Email
{
    public class EmailSendResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public static EmailSendResult Ok() => new EmailSendResult { Success = true };
        public static EmailSendResult Fail(string errorMessage) => new EmailSendResult { Success = false, ErrorMessage = errorMessage };
    }

    // Values from the (possibly unsaved) settings form, used by SendTestAsync so "Gui thu"
    // verifies what the admin is currently typing rather than whatever is already in the DB.
    public class SmtpTestSettings
    {
        public string? SmtpProvider { get; set; }
        public string? SmtpHost { get; set; }
        public int? SmtpPort { get; set; }
        public bool SmtpUseSsl { get; set; }
        public string? SmtpUsername { get; set; }
        // Leave null/empty to mean "use the password already saved in the database".
        public string? SmtpPassword { get; set; }
        public string? SmtpSenderName { get; set; }
        public string? NotificationEmails { get; set; }
    }

    // Sends notification emails using the SMTP configuration stored on AppSetting
    // (set from Admin > Cai dat chung, either a hosting mailbox or a Gmail account).
    public interface IEmailService
    {
        // Sends to the staff NotificationEmails list configured in Cai dat chung.
        Task<EmailSendResult> SendAsync(string subject, string htmlBody, CancellationToken cancellationToken = default);

        // Sends to a single arbitrary address (e.g. the customer who submitted a form),
        // using the same stored SMTP configuration as SendAsync.
        Task<EmailSendResult> SendToAsync(string recipientEmail, string subject, string htmlBody, CancellationToken cancellationToken = default);

        // Tests connectivity using the values currently on the settings form (not yet saved).
        Task<EmailSendResult> SendTestAsync(SmtpTestSettings overrideSettings, CancellationToken cancellationToken = default);
    }
}
