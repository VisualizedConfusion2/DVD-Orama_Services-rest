namespace DVD_Orama_Services_rest.Models.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
