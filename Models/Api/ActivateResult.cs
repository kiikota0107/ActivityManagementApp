namespace ActivityManagementApp.Models.Api
{
    public class ActivateResult
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }
        public string? Token { get; private set; }

        private ActivateResult(bool isSuccess, string? error, string? token)
        {
            IsSuccess = isSuccess;
            ErrorMessage = error;
            Token = token;
        }

        public static ActivateResult Success(string token)
            => new ActivateResult(true, null, token);

        public static ActivateResult Fail(string error)
            => new ActivateResult(false, error, null);
    }
}
