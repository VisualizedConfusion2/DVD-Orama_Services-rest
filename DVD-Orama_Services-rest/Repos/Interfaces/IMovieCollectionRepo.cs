using DVD_Orama_Services_rest.Models.DTOs;

namespace DVD_Orama_Services_rest.Repos.Interfaces
{
    public interface IMovieCollectionRepo
    {
        Task<List<MovieCollectionDto>> GetAllCollectionsAsync(int userId);
        Task<MovieCollectionDto?> GetCollectionByIdAsync(int userId, int collectionId);
        Task<int> CreateCollectionAsync(int userId, CreateMovieCollectionDto dto);
        Task<bool> UpdateCollectionAsync(int userId, int collectionId, CreateMovieCollectionDto dto);
        Task<bool> DeleteCollectionAsync(int userId, int collectionId);
        Task<bool> AddMovieToCollectionAsync(int userId, int collectionId, int movieId);
        Task<bool> RemoveMovieFromCollectionAsync(int userId, int collectionId, int movieId);
    }
}