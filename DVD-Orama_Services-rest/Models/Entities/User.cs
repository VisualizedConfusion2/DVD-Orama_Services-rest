using System.ComponentModel.DataAnnotations.Schema;

namespace DVD_Orama_Services_rest.Models.Entities
{
    public class User
    {
        [Column("UserId")]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirebaseUid { get; set; }
    }
}
