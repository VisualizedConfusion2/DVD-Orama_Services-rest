using DVD_Orama_Services_rest.Models.DTOs;

namespace DVD_Orama_Services_rest.Repos.Interfaces
{
    public interface IMovieRepo
    {
        Task<List<MovieDto>> GetAllMoviesAsync();
        Task<MovieDto?> GetMovieByIdAsync(int movieId);
        Task<int> CreateMovieAsync(CreateMovieDto dto);
        Task<bool> UpdateMovieAsync(int movieId, CreateMovieDto dto);
        Task<bool> DeleteMovieAsync(int movieId);
        Task<List<MovieDto>> SearchMoviesAsync(MovieSearchDto dto);

    }
}