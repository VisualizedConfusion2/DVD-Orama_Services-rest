namespace DVD_Orama_Services_rest.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<MovieCollectionItem> Movies { get; set; } = new List<MovieCollectionItem>();
    }
}
