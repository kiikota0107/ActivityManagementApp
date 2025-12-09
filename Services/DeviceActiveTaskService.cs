using ActivityManagementApp.Data;
using ActivityManagementApp.Models.Api;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Services
{
    public class DeviceActiveTaskService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public DeviceActiveTaskService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ActiveTaskResult> GetActiveTaskAsync(string userId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var task = await context.ActivityLogs
                .Include(x => x.CategoryMaster)
                .Where(x => x.UserId == userId && x.EndDateTime == default)
                .OrderByDescending(x => x.StartDateTime)
                .FirstOrDefaultAsync();

            if (task?.CategoryMaster?.SkipAppLockOnActiveFlg == true)
                return ActiveTaskResult.Allow();

            return ActiveTaskResult.Deny();
        }
    }
}
