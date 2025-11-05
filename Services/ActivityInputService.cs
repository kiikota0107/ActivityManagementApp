using Microsoft.EntityFrameworkCore;
using ActivityManagementApp.Data;
using ActivityManagementApp.Models;

namespace ActivityManagementApp.Services
{
    public class ActivityInputService
    {
        private readonly ApplicationDbContext _context;

        public ActivityInputService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InsertActivityLogsAsync(ActivityLogs newActivityLogs)
        {
            _context.ActivityLogs.Add(newActivityLogs);
            await _context.SaveChangesAsync();
        }
    }
}
