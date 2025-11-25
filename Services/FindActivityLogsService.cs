using ActivityManagementApp.Data;
using ActivityManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Services
{
    public class FindActivityLogsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly IConfiguration _config;

        public FindActivityLogsService(ApplicationDbContext context, UserService userService, IConfiguration config)
        {
            _context = context;
            _userService = userService;
            _config = config;
        }

        public async Task<ActivityLogs?> FindActivityLogsByIdAsync(int Id)
        {
            var currentUserId = await _userService.GetUserIdAsync();
            if (currentUserId == null) return null;

            ActivityLogs? activityLogs = await _context.ActivityLogs
                                        .Include(x => x.CategoryMaster)
                                            .ThenInclude(cm => cm!.CategoryTypeMaster)
                                        .Where(x => x.UserId == currentUserId)
                                        .FirstOrDefaultAsync(x => x.Id == Id);

            var guestUserId = await _userService.GetUserIdByEmailAsync(_config["GuestUser:Email"]);

            if (currentUserId == guestUserId)
            {
                var demoUserId = await _userService.GetUserIdByEmailAsync(_config["DemoOwner:Email"]);

                activityLogs = await _context.ActivityLogs
                                            .Include(x => x.CategoryMaster)
                                                .ThenInclude(cm => cm!.CategoryTypeMaster)
                                            .Where(x => x.UserId == demoUserId)
                                            .FirstOrDefaultAsync(x => x.Id == Id);
            }

            return activityLogs;
        }

        public async Task<List<ActivityLogs>> FindActivityLogsByDatgetDateAsync(DateTime targetDate)
        {
            var currentUserId = await _userService.GetUserIdAsync();
            if (currentUserId == null)
            {
                return new List<ActivityLogs>();
            }

            List<ActivityLogs> activityLogs = await _context.ActivityLogs
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

                List<ActivityLogs> demoLogs = await _context.ActivityLogs
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