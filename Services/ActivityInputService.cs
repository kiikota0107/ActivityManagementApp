using ActivityManagementApp.Data;
using ActivityManagementApp.Domain.Validators;
using ActivityManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Services
{
    public class ActivityInputService
    {
        IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserService _userService;
        private readonly TimeZoneService _timeZoneService;
        private readonly ActivityLogValidator _activityLogValidater;

        public ActivityInputService(IDbContextFactory<ApplicationDbContext> contextFactory, UserService userService, TimeZoneService timeZoneService, ActivityLogValidator activityLogValidator)
        {
            _contextFactory = contextFactory;
            _userService = userService;
            _timeZoneService = timeZoneService;
            _activityLogValidater = activityLogValidator;
        }

        public async Task<ActivityLogs?> FindProgressActivityLogsAsync()
        {
            var userId = await _userService.GetUserIdAsync();

            if (userId == null)
            {
                throw new Exception("ユーザーがログインしていません。");
            }

            using var context = await _contextFactory.CreateDbContextAsync();

            ActivityLogs? progressActivity = await context.ActivityLogs
                                                .Include(x => x.CategoryMaster)
                                                .ThenInclude(x => x!.CategoryTypeMaster)
                                                .Where(x => x.UserId == userId && x.EndDateTime < x.StartDateTime)
                                                .FirstOrDefaultAsync();
            return progressActivity;
        }

        public async Task<CustomValidationResult> InsertActivityLogsAsync(int categoryMasterId)
        {
            var userId = await _userService.GetUserIdAsync();

            if (userId == null)
            {
                throw new Exception("ユーザーがログインしていません。");
            }
            
            using var context = await _contextFactory.CreateDbContextAsync();

            ActivityLogs? latestProgressActivity = await context.ActivityLogs
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(x => x.EndDateTime == DateTime.MinValue
                                                                           && x.UserId == userId);

            if (latestProgressActivity != null)
            {
                var result = CustomValidationResult.InValid("進行中のタスクがあるため、新規タスクを作成できません。");
                return result;
            }

            var newActivityLogs = new ActivityLogs();
            var jst = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
            newActivityLogs.StartDateTime = _timeZoneService.NowJst;
            newActivityLogs.CategoryMasterId = categoryMasterId;
            newActivityLogs.UserId = userId;

            context.ActivityLogs.Add(newActivityLogs);
            await context.SaveChangesAsync();

            return CustomValidationResult.Valid();
        }

        public async Task<CustomValidationResult> UpdateProgressActivityLogsTempAsync(ActivityLogs activityLogsInput)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            ActivityLogs? progressActivity = await context.ActivityLogs.FindAsync(activityLogsInput.Id);

            ActivityLogs? latestActivity = await context.ActivityLogs
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(x => x.Id == activityLogsInput.Id);

            if (progressActivity != null && latestActivity != null)
            {
                var result = _activityLogValidater.Validate(latestActivity);
                if (!result.IsValid)
                {
                    return result;
                }

                progressActivity.ActivityDetailTitle = activityLogsInput.ActivityDetailTitle;
                progressActivity.ActivityDetail = activityLogsInput.ActivityDetail;

                await context.SaveChangesAsync();

                return result;
            }

            return CustomValidationResult.InValid("対象のレコードが見つかりませんでした。");
        }

        public async Task<CustomValidationResult> UpdateProgressActivityLogsAsync(ActivityLogs activityLogsInput)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            ActivityLogs? progressActivity = await context.ActivityLogs.FindAsync(activityLogsInput.Id);

            ActivityLogs? latestActivity = await context.ActivityLogs
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(x => x.Id == activityLogsInput.Id);

            if (progressActivity != null && latestActivity != null)
            {
                var result = _activityLogValidater.Validate(latestActivity);
                if (!result.IsValid)
                {
                    return result;
                }

                var jst = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
                progressActivity.EndDateTime = _timeZoneService.NowJst;

                if (progressActivity.StartDateTime.Date != progressActivity.EndDateTime.Date)
                {
                    // 進行中タスクのレコードの終了日時を、開始日の最終時間で更新
                    progressActivity.EndDateTime = progressActivity.StartDateTime.Date.AddDays(1).AddTicks(-1);

                    // 開始日が跨いだ日付のレコードを新規作成
                    ActivityLogs newDaysActivity = new ActivityLogs();
                    newDaysActivity.CategoryMasterId = progressActivity.CategoryMasterId;
                    newDaysActivity.StartDateTime = _timeZoneService.NowJst.Date;
                    newDaysActivity.EndDateTime = _timeZoneService.NowJst;
                    newDaysActivity.UserId = progressActivity.UserId;

                    string customFormatStartTimeForNewDays = newDaysActivity.StartDateTime.ToShortTimeString();
                    string customFormatEndTimeForNewDays = newDaysActivity.EndDateTime.ToShortTimeString();
                    TimeSpan diffForNewDays = DateTime.Parse(customFormatEndTimeForNewDays) - DateTime.Parse(customFormatStartTimeForNewDays);
                    newDaysActivity.PassingRoundMinutes = Math.Round(diffForNewDays.TotalMinutes);
                    context.ActivityLogs.Add(newDaysActivity);
                }

                string customFormatStartTime = progressActivity.StartDateTime.ToShortTimeString();
                string customFormatEndTime = progressActivity.EndDateTime.ToShortTimeString();
                TimeSpan diff = DateTime.Parse(customFormatEndTime) - DateTime.Parse(customFormatStartTime);
                progressActivity.PassingRoundMinutes = Math.Round(diff.TotalMinutes);
                progressActivity.ActivityDetailTitle = activityLogsInput.ActivityDetailTitle;
                progressActivity.ActivityDetail = activityLogsInput.ActivityDetail;

                await context.SaveChangesAsync();

                return result;
            }

            return CustomValidationResult.InValid("対象のレコードが見つかりませんでした。");
        }

        public async Task<CustomValidationResult> UpdateActivityDetailAsync(ActivityLogs inputActivityLogs)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            ActivityLogs? targetActivity = await context.ActivityLogs.FindAsync(inputActivityLogs.Id);

            if(targetActivity != null)
            {
                var userId = await _userService.GetUserIdAsync();

                if (userId != targetActivity.UserId) return CustomValidationResult.InValid("デモデータのため、更新できません");

                targetActivity.ActivityDetailTitle = inputActivityLogs.ActivityDetailTitle;
                targetActivity.ActivityDetail = inputActivityLogs.ActivityDetail;
                await context.SaveChangesAsync();
                return CustomValidationResult.Valid();
            }

            return CustomValidationResult.InValid("更新対象のレコードが見つかりませんでした。");
        }

        public async Task<CustomValidationResult> DeleteProgressActivityLogsAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            ActivityLogs? progressActivity = await context.ActivityLogs.FindAsync(id);

            ActivityLogs? latestActivity = await context.ActivityLogs
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(x => x.Id == id);

            if (progressActivity != null && latestActivity != null)
            {
                var result = _activityLogValidater.Validate(latestActivity);
                if (!result.IsValid)
                {
                    return result;
                }

                context.ActivityLogs.Remove(progressActivity);
                await context.SaveChangesAsync();

                return result;
            }

            return CustomValidationResult.InValid("対象のレコードが見つかりませんでした。");
        }
    }
}
