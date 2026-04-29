namespace DVD_Orama_Services_rest.Models.Entities
{
    public class MovieCollectionItem
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
