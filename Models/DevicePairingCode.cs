namespace ActivityManagementApp.Models
{
    public class DevicePairingCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string? DeviceName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
