using ActivityManagementApp.Models;

namespace ActivityManagementApp.Domain.Validators
{
    public class ActivityLogValidator
    {
        public CustomValidationResult Validate(ActivityLogs activityLogs)
        {
            var validators = new List<Func<ActivityLogs, CustomValidationResult>>
            {
                 ValidateNotAlreadyEnded
            };

            foreach (var validate in validators)
            {
                var result = validate(activityLogs);
                if (!result.IsValid)
                {
                    return result;
                }
            }

            return CustomValidationResult.Valid();
        }

        private CustomValidationResult ValidateNotAlreadyEnded(ActivityLogs activityLogs)
        {
            if (activityLogs.EndDateTime != DateTime.MinValue)
            {
                return CustomValidationResult.InValid("すでに終了しているタスクは更新できません。");
            }

            return CustomValidationResult.Valid();
        }
    }
}
