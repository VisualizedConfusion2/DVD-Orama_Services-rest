namespace DVD_Orama_Services_rest.Models.DTOs
{
    public class MovieCollectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public List<MovieDto> Movies { get; set; } = new();
    }
}