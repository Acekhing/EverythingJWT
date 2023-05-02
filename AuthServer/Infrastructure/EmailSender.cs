using AuthServer.Execeptions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AuthServer.Infrastructure
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<EmailSender> logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            this.configuration = configuration;
            this.logger = logger;

            logger.LogInformation($"{nameof(EmailSender)} called...");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            logger.LogInformation($"{nameof(SendEmailAsync)} called...");

            // Check if Email sender apiKey exist
            string? apiKey = configuration["EmailSettings:Key"];

            if (string.IsNullOrEmpty(apiKey))
            {
                logger.LogError($"Invalid apikey: {apiKey}");
                throw new ApiKeyException();
            }
            

            // Send the email
            await Execute(apiKey, subject, message, toEmail);
        }

        private async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            logger.LogInformation($"{nameof(Execute)} called...");

            // Construct a sendGrid client
            var client = new SendGridClient(apiKey);

            // COnstruct message
            var emailToSend = new SendGridMessage()
            {
                From = new EmailAddress("charlesannorblay@gmail.com", "Charles"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            // Attach email to receiver
            emailToSend.AddTo(toEmail);

            emailToSend.SetClickTracking(false, false);

            // Queue email
            var response = await client.SendEmailAsync(emailToSend);

            logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
            logger.LogInformation(response.Body.ToString());
        }
    }
}
