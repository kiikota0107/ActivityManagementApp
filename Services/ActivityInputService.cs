using ActivityManagementApp.Data;
using ActivityManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Services
{
    public class ActivityInputService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public ActivityInputService(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<ActivityLogs?> FindProgressActivityLogsAsync()
        {
            var userId = await _userService.GetUserIdAsync();

            if (userId == null)
            {
                throw new Exception("ユーザーがログインしていません。");
            }

            ActivityLogs? progressActivity = await _context.ActivityLogs
                                                .Include(x => x.CategoryMaster)
                                                .ThenInclude(x => x!.CategoryTypeMaster)
                                                .Where(x => x.UserId == userId && x.EndDateTime < x.StartDateTime)
                                                .FirstOrDefaultAsync();
            return progressActivity;
        }

        public async Task InsertActivityLogsAsync(ActivityLogs newActivityLogs)
        {
            var userId = await _userService.GetUserIdAsync();

            if (userId == null)
            {
                throw new Exception("ユーザーがログインしていません。");
            }

            newActivityLogs.UserId = userId;
            _context.ActivityLogs.Add(newActivityLogs);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProgressActivityLogsTempAsync(ActivityLogs activityLogsInput)
        {
            ActivityLogs? progressActivity = await _context.ActivityLogs.FindAsync(activityLogsInput.Id);

            if (progressActivity != null)
            {
                progressActivity.ActivityDetailTitle = activityLogsInput.ActivityDetailTitle;
                progressActivity.ActivityDetail = activityLogsInput.ActivityDetail;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProgressActivityLogsAsync(ActivityLogs activityLogsInput)
        {
            ActivityLogs? progressActivity = await _context.ActivityLogs.FindAsync(activityLogsInput.Id);

            if (progressActivity != null)
            {
                var jst = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
                progressActivity.EndDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, jst);

                if (progressActivity.StartDateTime.Date != progressActivity.EndDateTime.Date)
                {
                    // 進行中タスクのレコードの終了日時を、開始日の最終時間で更新
                    progressActivity.EndDateTime = progressActivity.StartDateTime.Date.AddDays(1).AddTicks(-1);

                    // 開始日が跨いだ日付のレコードを新規作成
                    ActivityLogs newDaysActivity = new ActivityLogs();
                    newDaysActivity.CategoryMasterId = progressActivity.CategoryMasterId;
                    newDaysActivity.StartDateTime = DateTime.Today;
                    newDaysActivity.EndDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, jst);
                    newDaysActivity.UserId = progressActivity.UserId;

                    string customFormatStartTimeForNewDays = newDaysActivity.StartDateTime.ToShortTimeString();
                    string customFormatEndTimeForNewDays = newDaysActivity.EndDateTime.ToShortTimeString();
                    TimeSpan diffForNewDays = DateTime.Parse(customFormatEndTimeForNewDays) - DateTime.Parse(customFormatStartTimeForNewDays);
                    newDaysActivity.PassingRoundMinutes = Math.Round(diffForNewDays.TotalMinutes);
                    _context.ActivityLogs.Add(newDaysActivity);
                }

                string customFormatStartTime = progressActivity.StartDateTime.ToShortTimeString();
                string customFormatEndTime = progressActivity.EndDateTime.ToShortTimeString();
                TimeSpan diff = DateTime.Parse(customFormatEndTime) - DateTime.Parse(customFormatStartTime);
                progressActivity.PassingRoundMinutes = Math.Round(diff.TotalMinutes);
                progressActivity.ActivityDetailTitle = activityLogsInput.ActivityDetailTitle;
                progressActivity.ActivityDetail = activityLogsInput.ActivityDetail;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateActivityDetailAsync(ActivityLogs inputActivityLogs)
        {
            ActivityLogs? targetActivity = await _context.ActivityLogs.FindAsync(inputActivityLogs.Id);
            if(targetActivity != null)
            {
                targetActivity.ActivityDetailTitle = inputActivityLogs.ActivityDetailTitle;
                targetActivity.ActivityDetail = inputActivityLogs.ActivityDetail;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProgressActivityLogsAsync(int id)
        {
            ActivityLogs? progressActivity = await _context.ActivityLogs.FindAsync(id);
            if (progressActivity != null)
            {
                _context.ActivityLogs.Remove(progressActivity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
