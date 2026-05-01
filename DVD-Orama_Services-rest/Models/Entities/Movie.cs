namespace DVD_Orama_Services_rest.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int PublicationYear { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
        public List<string> Genres { get; set; } = new();
        public List<string> Actors { get; set; } = new();
        public List<StreamingLocation> StreamingServices { get; set; } = new();
        public List<long> EANs { get; set; } = new();
    }
}
