namespace BusinessWallet.DTOs
{
    public class RoleUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? CanIssue { get; set; }
        public bool? CanReceive { get; set; }
        public bool? CanStore { get; set; }
        public bool? CanView { get; set; }
        public bool? CanPresent { get; set; }
        public bool? CanVerify { get; set; }
        public bool? CanRevoke { get; set; }
    }
}
