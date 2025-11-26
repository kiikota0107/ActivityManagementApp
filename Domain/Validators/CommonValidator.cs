namespace ActivityManagementApp.Domain.Validators
{
    public static class CommonValidator
    {
        public static CustomValidationResult ValidateUserIsOwner(string recordUserId, string currentUserId)
        {
            if (recordUserId != currentUserId)
                return CustomValidationResult.InValid("登録時のユーザと異なるユーザによる更新はできません。");
            return CustomValidationResult.Valid();
        }
    }
}