using System.ComponentModel.DataAnnotations.Schema;

namespace DVD_Orama_Services_rest.Models.Entities
{
    public class User
    {
        [Column("UserId")]  // ← explicitly maps to UserId in DB
        public int Id { get; set; }  // ← matches DB column name
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
