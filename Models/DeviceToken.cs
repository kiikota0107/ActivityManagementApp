namespace ActivityManagementApp.Models
{
    public class DeviceToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string? DeviceName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}
