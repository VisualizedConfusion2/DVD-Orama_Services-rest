using DVD_Orama_Services_rest.Models.Entities;

namespace DVD_Orama_Services_rest.Models.DTOs
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int PublicationYear { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
        public List<string> Genres { get; set; } = new();
        public List<string> Actors { get; set; } = new();
        public List<StreamingLocation> StreamingLocations { get; set; } = new();
        public List<long> EANs { get; set; } = new();
    }
}
