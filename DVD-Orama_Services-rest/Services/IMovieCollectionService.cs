using DVD_Orama_Services_rest.Models.DTOs;

namespace DVD_Orama_Services_rest.Services
{
    public interface IMovieCollectionService
    {
        Task AddMovieAsync(int userId, string barcode);
        Task<List<MovieDto>> GetUserMoviesAsync(int userId);
        Task<MovieDto?> GetMovieByIdAsync(int userId, int movieId);
        Task<bool> UpdateMovieAsync(int userId, int movieId, string newBarcode);
        Task<bool> DeleteMovieAsync(int userId, int movieId);
    }
}