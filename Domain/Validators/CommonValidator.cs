using ActivityManagementApp.Models;

namespace ActivityManagementApp.Domain.Validators
{
    public static class CommonValidator
    {
        public static CustomValidationResult ValidateUserIsOwner(string recordUserId, string currentUserId)
        {
            if (recordUserId != currentUserId)
                return CustomValidationResult.InValid("登録時のユーザと異なるユーザによる更新/削除はできません。");
            return CustomValidationResult.Valid();
        }

        public static CustomValidationResult ValidateMixUser(IEnumerable<IHasUserId> models)
        {
            var userIds = models.Select(x => x.UserId).Distinct();

            if (userIds.Count() > 1)
                return CustomValidationResult.InValid("登録したユーザが異なるレコードが含まれているため更新/削除はできません。");

            return CustomValidationResult.Valid();
        }
    }
}