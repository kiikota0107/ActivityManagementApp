namespace ActivityManagementApp.Models.Api
{
    public class ActivateRequest
    {
        public string Code { get; set; } = default!;
        public string? DeviceName { get; set; }
    }
}
