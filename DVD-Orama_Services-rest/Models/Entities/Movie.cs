namespace DVD_Orama_Services_rest.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; } //stored in seconds
        public int PublicationYear { get; set; } 
        public List<string> Genres { get; set; } //Could be an enum, i just don't feel like writing out that many genres
        public List<string> Actors { get; set; } //In a more complete edition of this product you might have an actor class
        public List<string> StreamingServices { get; set; } //Could also have been an enum
        public string Barcode { get; set; } = string.Empty;



    }
}
