using System.ComponentModel.DataAnnotations.Schema;

public class Genre
{
    public int GenreId { get; set; }
    [Column("Genre")]
    public string GenreName { get; set; } = string.Empty;
}