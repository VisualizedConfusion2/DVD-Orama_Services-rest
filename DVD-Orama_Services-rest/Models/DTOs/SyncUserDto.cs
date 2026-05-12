namespace DVD_Orama_Services_rest.Models.DTOs
{
    public class SyncUserDto
    {
        public string FirebaseUid { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
