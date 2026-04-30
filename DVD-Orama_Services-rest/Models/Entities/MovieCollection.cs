namespace DVD_Orama_Services_rest.Models.Entities
{
    public class MovieCollectionItem
    {
        public int Id { get; set; }
        //public string Barcode { get; set; } = string.Empty;

        //public int UserId { get; set; }
        public User Owner { get; set; } = null!;
        public bool IsPublic { get; set; } = false;
        public List<Movie> Movies { get; set; }
        public List<int> AdminIds { get; set; }
        public List<int> RegUserIds { get; set; }
        public List<int> ViewerIds { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
