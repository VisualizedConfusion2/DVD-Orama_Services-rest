namespace DVD_Orama_Services_rest.Models.DTOs
{
    public class MovieSearchDto
    {
        public string? Title { get; set; }
        public int? PublicationYear { get; set; }
        public List<string>? Genres { get; set; }
        public List<string>? Actors { get; set; }
        public List<string>? StreamingServices { get; set; }
    }
}
