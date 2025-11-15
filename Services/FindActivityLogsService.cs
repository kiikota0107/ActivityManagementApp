using ActivityManagementApp.Data;
using ActivityManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Services
{
    public class FindActivityLogsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public FindActivityLogsService(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<ActivityLogs?> FindActivityLogsByIdAsync(int Id)
        {
            var userId = await _userService.GetUserIdAsync();
            if (userId == null) return null;

            ActivityLogs? activityLogs = await _context.ActivityLogs
                                        .Include(x => x.CategoryMaster)
                                            .ThenInclude(cm => cm.CategoryTypeMaster)
                                        .Where(x => x.UserId == userId)
                                        .FirstOrDefaultAsync(x => x.Id == Id);
            return activityLogs;
        }

        public async Task<List<ActivityLogs>> FindActivityLogsByDatgetDateAsync(DateTime targetDate)
        {
            var userId = await _userService.GetUserIdAsync();
            if (userId == null)
            {
                return new List<ActivityLogs>();
            }

            List<ActivityLogs> activityLogs = await _context.ActivityLogs
                                                .Include(x => x.CategoryMaster)
                                                    .ThenInclude(cm => cm.CategoryTypeMaster)
                                                .Where(x => x.UserId == userId
                                                         && x.StartDateTime.Date == targetDate.Date
                                                         && x.EndDateTime != DateTime.MinValue)
                                                .OrderBy(x => x.StartDateTime)
                                                .ToListAsync();
            return activityLogs;
        }
    }
}