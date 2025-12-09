namespace ActivityManagementApp.Models.Api
{
    public class ActiveTaskResult
    {
        public bool SkipAppLock { get; set; }

        public static ActiveTaskResult Allow()
            => new ActiveTaskResult { SkipAppLock = true };

        public static ActiveTaskResult Deny()
            => new ActiveTaskResult { SkipAppLock =false };
    }
}
