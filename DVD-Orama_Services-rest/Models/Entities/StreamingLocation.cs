namespace DVD_Orama_Services_rest.Models.Entities
{
    public class StreamingLocation
    {
        public int StreamingServiceId { get; set; }
        public string StreamingServiceName { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public int MovieId { get; set; }
    }
}
