namespace ActivityManagementApp.Domain.Validators
{
    public class CustomValidationResult
    {
        public bool IsValid { get; }
        public string? ErrorMessage { get; }

        public static CustomValidationResult Valid() => new(true, null);
        public static CustomValidationResult InValid(string? errorMessage) => new(false, errorMessage);

        private CustomValidationResult(bool isValid, string? errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
    }
}
