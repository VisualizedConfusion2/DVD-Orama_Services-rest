using DVD_Orama_Services_rest.Data;
using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Models.Entities;
using DVD_Orama_Services_rest.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVD_Orama_Services_rest.Repos
{
    public class MovieCollectionRepo : IMovieCollectionRepo
    {
        private readonly AppDbContext _context;

        public MovieCollectionRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieCollectionDto>> GetAllCollectionsAsync(int userId)
        {
            return await _context.UserMovieCollectionMap
                .Where(u => u.UserId == userId)
                .Join(_context.MovieCollections,
                    u => u.MovieCollectionId,
                    c => c.Id,
                    (u, c) => c)
                .Select(c => new MovieCollectionDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsPublic = c.IsPublic,
                    Movies = _context.MovieCollectionsMoviesMap
                        .Where(m => m.MovieCollectionId == c.Id)
                        .Join(_context.Movies,
                            m => m.MovieId,
                            movie => movie.Id,
                            (m, movie) => new MovieDto
                            {
                                Id = movie.Id,
                                Title = movie.Title,
                                PosterUrl = movie.PosterUrl,
                                PublicationYear = movie.PublicationYear
                            })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<MovieCollectionDto?> GetCollectionByIdAsync(int userId, int collectionId)
        {
            var owns = await _context.UserMovieCollectionMap
                .AnyAsync(u => u.UserId == userId && u.MovieCollectionId == collectionId);
            if (!owns) return null;

            return await _context.MovieCollections
                .Where(c => c.Id == collectionId)
                .Select(c => new MovieCollectionDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsPublic = c.IsPublic,
                    Movies = _context.MovieCollectionsMoviesMap
                        .Where(m => m.MovieCollectionId == c.Id)
                        .Join(_context.Movies,
                            m => m.MovieId,
                            movie => movie.Id,
                            (m, movie) => new MovieDto
                            {
                                Id = movie.Id,
                                Title = movie.Title,
                                PosterUrl = movie.PosterUrl,
                                PublicationYear = movie.PublicationYear
                            })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateCollectionAsync(int userId, CreateMovieCollectionDto dto)
        {
            var collection = new MovieCollection
            {
                Name = dto.Name,
                IsPublic = dto.IsPublic
            };
            _context.MovieCollections.Add(collection);
            await _context.SaveChangesAsync();

            _context.UserMovieCollectionMap.Add(new UserMovieCollectionMap
            {
                UserId = userId,
                MovieCollectionId = collection.Id,
                RoleId = 1
            });
            await _context.SaveChangesAsync();
            return collection.Id;
        }

        public async Task<bool> UpdateCollectionAsync(int userId, int collectionId, CreateMovieCollectionDto dto)
        {
            var owns = await _context.UserMovieCollectionMap
                .AnyAsync(u => u.UserId == userId && u.MovieCollectionId == collectionId);
            if (!owns) return false;

            var collection = await _context.MovieCollections.FindAsync(collectionId);
            if (collection == null) return false;

            collection.Name = dto.Name;
            collection.IsPublic = dto.IsPublic;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCollectionAsync(int userId, int collectionId)
        {
            var map = await _context.UserMovieCollectionMap
                .FirstOrDefaultAsync(u => u.UserId == userId && u.MovieCollectionId == collectionId);
            if (map == null) return false;

            var collection = await _context.MovieCollections.FindAsync(collectionId);
            if (collection == null) return false;

            _context.UserMovieCollectionMap.Remove(map);
            _context.MovieCollections.Remove(collection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddMovieToCollectionAsync(int userId, int collectionId, int movieId)
        {
            var owns = await _context.UserMovieCollectionMap
                .AnyAsync(u => u.UserId == userId && u.MovieCollectionId == collectionId);
            if (!owns) return false;

            var exists = await _context.MovieCollectionsMoviesMap
                .AnyAsync(m => m.MovieCollectionId == collectionId && m.MovieId == movieId);
            if (exists) return false;

            _context.MovieCollectionsMoviesMap.Add(new MovieCollectionsMoviesMap
            {
                MovieCollectionId = collectionId,
                MovieId = movieId
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveMovieFromCollectionAsync(int userId, int collectionId, int movieId)
        {
            var owns = await _context.UserMovieCollectionMap
                .AnyAsync(u => u.UserId == userId && u.MovieCollectionId == collectionId);
            if (!owns) return false;

            var entry = await _context.MovieCollectionsMoviesMap
                .FirstOrDefaultAsync(m => m.MovieCollectionId == collectionId && m.MovieId == movieId);
            if (entry == null) return false;

            _context.MovieCollectionsMoviesMap.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}