using SendGrid;

namespace AuthServer.Infrastructure
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
