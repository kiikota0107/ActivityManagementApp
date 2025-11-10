using System.ComponentModel.DataAnnotations;

namespace ActivityManagementApp.Models
{
    public class ActivityLogs
    {
        // ★★★変更した場合はマイグレーション実行する★★★
        [Key]
        public int Id { get; set; }
        public string? Category { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double PassingRoundMinutes { get; set; } = 0;
        public string? ActivityDetail { get; set; }
    }
}
