namespace ActivityManagementApp.Models.Api
{
    public class VerifyTokenResult
    {
        public bool IsSuccess { get; }
        public string? UserId { get; }
        public string? ErrorMessage { get; }

        private VerifyTokenResult(bool isSuccess, string? userId, string? errorMessage)
        {
            IsSuccess = isSuccess;
            UserId = userId;
            ErrorMessage = errorMessage;
        }

        public static VerifyTokenResult Success(string userId)
            => new VerifyTokenResult(true, userId, null);

        public static VerifyTokenResult Fail(string errorMessage)
            => new VerifyTokenResult(false, null, errorMessage);
    }
}
