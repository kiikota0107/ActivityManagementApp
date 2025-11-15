using System.ComponentModel.DataAnnotations;

namespace ActivityManagementApp.ViewModels
{
    /// <summary>
    /// ユーザー認証まわりの ViewModel をまとめたクラス群。
    /// </summary>
    public static class AuthInputModels
    {
        // ================================
        // ■ Login
        // ================================
        public class LoginInputModel
        {
            [Required(ErrorMessage = "メールアドレスを入力してください。")]
            [EmailAddress(ErrorMessage = "メールアドレスの形式が正しくありません。")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "パスワードを入力してください。")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";

            [Display(Name = "ログイン状態を保持する")]
            public bool RememberMe { get; set; }
        }

        // ================================
        // ■ Register
        // ================================
        public class RegisterInputModel
        {
            [Required(ErrorMessage = "メールアドレスを入力してください。")]
            [EmailAddress(ErrorMessage = "メールアドレスの形式が正しくありません。")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "パスワードを入力してください。")]
            [StringLength(100, ErrorMessage = "{0} は {2} 文字以上で指定してください。", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";

            [Required(ErrorMessage = "確認用パスワードを入力してください。")]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "パスワードと確認用パスワードが一致しません。")]
            public string ConfirmPassword { get; set; } = "";
        }

        // ================================
        // ■ Forgot Password
        // ================================
        public class ForgotPasswordInputModel
        {
            [Required(ErrorMessage = "メールアドレスを入力してください。")]
            [EmailAddress(ErrorMessage = "メールアドレスの形式が正しくありません。")]
            public string Email { get; set; } = "";
        }

        // ================================
        // ■ Reset Password（メールリンクから遷移）
        // ================================
        public class ResetPasswordInputModel
        {
            [Required(ErrorMessage = "メールアドレスを入力してください。")]
            [EmailAddress(ErrorMessage = "メールアドレスの形式が正しくありません。")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "パスワードを入力してください。")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "6文字以上で入力してください。")]
            public string Password { get; set; } = "";

            [Required(ErrorMessage = "確認用パスワードを入力してください。")]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "パスワードが一致しません。")]
            public string ConfirmPassword { get; set; } = "";

            // Validation errors from Identity (e.g. password rules)
            public List<string> ModelValidationErrors { get; set; } = new();
        }
    }
}
