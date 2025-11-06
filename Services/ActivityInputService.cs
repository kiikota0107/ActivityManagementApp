using ActivityManagementApp.Data;
using ActivityManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Services
{
    public class ActivityInputService
    {
        private readonly ApplicationDbContext _context;

        public ActivityInputService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActivityLogs?> FindProgressActivityLogsAsync()
        {
            ActivityLogs? progressActivity = await _context.ActivityLogs.Where(x => x.EndDateTime < x.StartDateTime).FirstOrDefaultAsync();
            return progressActivity;
        }

        public async Task InsertActivityLogsAsync(ActivityLogs newActivityLogs)
        {
            _context.ActivityLogs.Add(newActivityLogs);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProgressActivityLogsAsync(int id)
        {
            ActivityLogs? progressActivity = await _context.ActivityLogs.FindAsync(id);
            if (progressActivity != null)
            {
                progressActivity.EndDateTime = DateTime.Now;
                string customFormatStartTime = progressActivity.StartDateTime.ToString("HH:mm");
                string customFormatEndTime = progressActivity.EndDateTime.ToString("HH:mm");
                TimeSpan diff = DateTime.Parse(customFormatEndTime) - DateTime.Parse(customFormatStartTime);
                progressActivity.PassingRoundMinutes = Math.Round(diff.TotalMinutes);
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
