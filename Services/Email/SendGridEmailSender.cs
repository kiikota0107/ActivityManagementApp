using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ActivityManagementApp.Services.Email
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SendGridEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("SendGrid API Key が設定されていません。");

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("kiikota0107@gmail.com", "ActivityManagementApp");
            var to = new EmailAddress(email);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            await client.SendEmailAsync(msg);
        }
    }
}
