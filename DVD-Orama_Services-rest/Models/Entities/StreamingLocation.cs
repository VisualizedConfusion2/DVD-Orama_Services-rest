namespace DVD_Orama_Services_rest.Models.Entities
{
    public class StreamingLocation
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public int MovieId { get; set; }
    }
}
