namespace Dentisty.Data.Services.Email
{
    public interface ISmtpCredentialProtector
    {
        string? Protect(string? plaintext);
        string? Unprotect(string? protectedText);
    }
}
