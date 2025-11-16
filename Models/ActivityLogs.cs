using System.ComponentModel.DataAnnotations;

namespace ActivityManagementApp.Models
{
    public class ActivityLogs
    {
        [Key]
        public int Id { get; set; }
        public int? CategoryMasterId { get; set; }
        public CategoryMaster? CategoryMaster { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double PassingRoundMinutes { get; set; }
        public string? ActivityDetailTitle { get; set; }
        public string? ActivityDetail { get; set; }
        [Required]
        public string UserId { get; set; } = default!;
    }
}
