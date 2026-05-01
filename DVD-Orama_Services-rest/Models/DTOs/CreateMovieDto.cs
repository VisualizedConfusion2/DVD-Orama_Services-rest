namespace DVD_Orama_Services_rest.Models.DTOs
{
    public class CreateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int PublicationYear { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
    }
}