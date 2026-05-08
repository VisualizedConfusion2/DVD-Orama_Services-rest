namespace DVD_Orama_Services_rest.Models.DTOs
{
    public class CreateMovieCollectionDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
    }
}
