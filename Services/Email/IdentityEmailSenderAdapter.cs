using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using ActivityManagementApp.Data;

namespace ActivityManagementApp.Services.Email
{
    public class IdentityEmailSenderAdapter : IEmailSender<ApplicationUser>
    {
        private readonly IEmailSender _emailSender;

        public IdentityEmailSenderAdapter(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            return _emailSender.SendEmailAsync(
                email,
                "メールアドレスの確認",
                $"<p>こちらをクリックして認証を完了してください:</p><p><a href='{confirmationLink}'>メールを確認</a></p>"
                );
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            return _emailSender.SendEmailAsync(
                email,
                "パスワードリセット用セキュリティコード",
                $"<p>以下のコードを入力してください:</p><h2>{resetCode}</h2>"
            );
        }
        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string callbackUrl)
        {
            return _emailSender.SendEmailAsync(
                email,
                "パスワードリセット",
                $@"<p>以下のリンクからパスワード再設定を行ってください:</p>
           <p><a href=""{callbackUrl}"">パスワードを再設定する</a></p>"
            );
        }
    }
}
