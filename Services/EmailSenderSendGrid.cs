using ActivityManagementApp.Data;
using Microsoft.AspNetCore.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace ActivityManagementApp.Services;

public class EmailSenderSendGrid : IEmailSender<ApplicationUser>
{
    private readonly string _apiKey;

    public EmailSenderSendGrid(IConfiguration config)
    {
        _apiKey = config["SendGrid:ApiKey"]
            ?? throw new Exception("SendGrid API Key が設定されていません。");
    }

    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        await SendEmailAsync(
            email,
            "メールアドレス確認",
            $"以下のリンクをクリックしてメールアドレスを確認してください:<br><a href='{confirmationLink}'>メールアドレス確認</a>"
        );
    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        await SendEmailAsync(
            email,
            "パスワードリセット",
            $"以下のリンクをクリックしてパスワードを再設定してください:<br><a href='{resetLink}'>パスワードリセット</a>"
        );
    }

    private async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        var client = new SendGridClient(_apiKey);

        var from = new EmailAddress("no-reply@yourapp.com", "ActivityManagementApp");
        var to = new EmailAddress(toEmail);

        var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

        await client.SendEmailAsync(msg);
    }

    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        await SendEmailAsync(
            email,
            "パスワードリセット用セキュリティコード",
            $"以下のコードを入力してください:<br><h2>{resetCode}</h2>"
        );
    }
}
