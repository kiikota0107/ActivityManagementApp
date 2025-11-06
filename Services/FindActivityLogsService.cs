using ActivityManagementApp.Data;
using ActivityManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Services
{
    public class FindActivityLogsService
    {
        private readonly ApplicationDbContext _context;

        public FindActivityLogsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ActivityLogs>> FindActivityLogs(DateTime targetDate)
        {
            List<ActivityLogs> activityLogs = await _context.ActivityLogs.Where(x => ((x.StartDateTime > targetDate && x.StartDateTime < targetDate.AddDays(1))
                                                                                  || (x.EndDateTime > targetDate && x.EndDateTime < targetDate.AddDays(1)))
                                                                                  && x.EndDateTime > x.StartDateTime).ToListAsync();
            return activityLogs;
        }
    }
}