using Microsoft.AspNetCore.Identity;

public class JapaneseIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError PasswordTooShort(int length)
        => new() { Code = nameof(PasswordTooShort), Description = $"パスワードは最低 {length} 文字必要です。" };

    public override IdentityError PasswordRequiresDigit()
        => new() { Code = nameof(PasswordRequiresDigit), Description = "パスワードには数字を1文字以上含める必要があります。" };

    public override IdentityError PasswordRequiresUpper()
        => new() { Code = nameof(PasswordRequiresUpper), Description = "パスワードには大文字を1文字以上含める必要があります。" };

    public override IdentityError PasswordRequiresLower()
        => new() { Code = nameof(PasswordRequiresLower), Description = "パスワードには小文字を1文字以上含める必要があります。" };

    public override IdentityError DuplicateUserName(string userName)
        => new() { Code = nameof(DuplicateUserName), Description = $"メールアドレス '{userName}' は既に登録されています。" };

    public override IdentityError InvalidUserName(string? userName)
        => new() { Code = nameof(InvalidUserName), Description = $"メールアドレス '{userName}' は無効です。" };

    public override IdentityError PasswordRequiresNonAlphanumeric()
        => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "パスワードには記号を含める必要があります。" };

}
