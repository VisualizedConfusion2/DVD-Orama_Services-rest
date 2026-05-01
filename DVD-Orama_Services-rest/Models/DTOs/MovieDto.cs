namespace DVD_Orama_Services_rest.Models.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
    }
}
