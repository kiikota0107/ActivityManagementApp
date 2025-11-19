namespace ActivityManagementApp.Services
{
    public class TimeZoneService
    {
        private readonly TimeZoneInfo jst = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");

        public DateTime NowJst => TimeZoneInfo.ConvertTime(DateTime.UtcNow, jst);

        public DateTime ToJst(DateTime utc)
            => TimeZoneInfo.ConvertTime(utc, jst);
    }
}
