namespace DVD_Orama_Services_rest.Models.Entities
{
    public class MovieCollection
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public bool IsPublic { get; set; } = false;
        public List<Movie> Movies { get; set; }
        public List<int> AdminIds { get; set; }
        public List<int> RegUserIds { get; set; }
        public List<int> ViewerIds { get; set; }
    }
}
