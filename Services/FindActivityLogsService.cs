using ActivityManagementApp.Data;
using ActivityManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Services
{
    public class FindActivityLogsService
    {
        IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserService _userService;
        private readonly IConfiguration _config;

        public FindActivityLogsService(IDbContextFactory<ApplicationDbContext> contextFactory, UserService userService, IConfiguration config)
        {
            _contextFactory = contextFactory;
            _userService = userService;
            _config = config;
        }

        public async Task<ActivityLogs?> FindActivityLogsByIdAsync(int Id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var currentUserId = await _userService.GetUserIdAsync();
            if (currentUserId == null) return null;

            ActivityLogs? activityLogs = await context.ActivityLogs
                                        .Include(x => x.CategoryMaster)
                                            .ThenInclude(cm => cm!.CategoryTypeMaster)
                                        .Where(x => x.UserId == currentUserId)
                                        .FirstOrDefaultAsync(x => x.Id == Id);

            var guestUserId = await _userService.GetUserIdByEmailAsync(_config["GuestUser:Email"]);

            if (activityLogs == null && currentUserId == guestUserId)
            {
                var demoUserId = await _userService.GetUserIdByEmailAsync(_config["DemoOwner:Email"]);

                activityLogs = await context.ActivityLogs
                                            .Include(x => x.CategoryMaster)
                                                .ThenInclude(cm => cm!.CategoryTypeMaster)
                                            .Where(x => x.UserId == demoUserId)
                                            .FirstOrDefaultAsync(x => x.Id == Id);
            }

            return activityLogs;
        }

        public async Task<List<ActivityLogs>> FindActivityLogsByDatgetDateAsync(DateTime targetDate)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var currentUserId = await _userService.GetUserIdAsync();
            if (currentUserId == null)
            {
                return new List<ActivityLogs>();
            }

            List<ActivityLogs> activityLogs = await context.ActivityLogs
                                                .Include(x => x.CategoryMaster)
                                                    .ThenInclude(cm => cm!.CategoryTypeMaster)
                                                .Where(x => x.UserId == currentUserId
                                                         && x.StartDateTime.Date == targetDate.Date
                                                         && x.EndDateTime != DateTime.MinValue)
                                                .OrderBy(x => x.StartDateTime)
                                                .ToListAsync();

            var guestUserId = await _userService.GetUserIdByEmailAsync(_config["GuestUser:Email"]);

            if (currentUserId == guestUserId)
            {
                var demoUserId = await _userService.GetUserIdByEmailAsync(_config["DemoOwner:Email"]);

                List<ActivityLogs> demoLogs = await context.ActivityLogs
                                                .Include(x => x.CategoryMaster)
                                                    .ThenInclude(cm => cm!.CategoryTypeMaster)
                                                .Where(x => x.UserId == demoUserId
                                                         && x.StartDateTime.Date == targetDate.Date
                                                         && x.EndDateTime != DateTime.MinValue
                                                         )
                                                .OrderBy(x => x.StartDateTime)
                                                .ToListAsync();

                activityLogs.AddRange(demoLogs);
                activityLogs.OrderBy(x => x.StartDateTime);
            }

            return activityLogs;
        }
    }
}