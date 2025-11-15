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

        public async Task<ActivityLogs?> FindActivityLogsById(int Id)
        {
            var userId = await _userService.GetUserIdAsync();
            if (userId == null) return null;

            ActivityLogs activityLogs = await _context.ActivityLogs.SingleAsync(x => x.Id == Id && x.UserId == userId);
            return activityLogs;
        }

        public async Task<List<ActivityLogs>> FindActivityLogsByDatgetDate(DateTime targetDate)
        {
            var userId = await _userService.GetUserIdAsync();
            if (userId == null)
            {
                return new List<ActivityLogs>();
            }

            List<ActivityLogs> activityLogs = await _context.ActivityLogs.Where(x => (x.UserId == userId
                                                                                  && (x.StartDateTime > targetDate && x.StartDateTime < targetDate.AddDays(1))
                                                                                  || (x.EndDateTime > targetDate && x.EndDateTime < targetDate.AddDays(1)))
                                                                                  && x.EndDateTime > x.StartDateTime).OrderBy(y => y.StartDateTime).ToListAsync();
            return activityLogs;
        }
    }
}