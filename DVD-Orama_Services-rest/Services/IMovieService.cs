using DVD_Orama_Services_rest.Models.DTOs;

namespace DVD_Orama_Services_rest.Services
{
    public interface IMovieService
    {
        Task<int> UpsertMovieAsync(CreateMovieDto dto);
    }
}
