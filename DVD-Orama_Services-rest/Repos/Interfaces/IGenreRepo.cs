using DVD_Orama_Services_rest.Models.DTOs;

namespace DVD_Orama_Services_rest.Repos.Interfaces
{
    public interface IGenreRepo
    {
        Task<List<GenreDto>> GetAllGenresAsync();
    }
}