using DVD_Orama_Services_rest.Data;
using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DVD_Orama_Services_rest.Services
{
    public class MovieCollectionService : IMovieCollectionService
    {
        private readonly AppDbContext _context;

        public MovieCollectionService(AppDbContext context)
        {
            _context = context;
        }

        private static void ValidateBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                throw new ArgumentException("Barcode cannot be empty.");

            if (barcode.Length < 8 || barcode.Length > 14)
                throw new ArgumentException("Invalid barcode format. Must be 8–14 characters.");
        }

        // CREATE
        public async Task AddMovieAsync(int userId, string barcode)
        {
            ValidateBarcode(barcode);

            var movie = new MovieCollectionItem
            {
                Barcode = barcode,
                UserId = userId
            };

            _context.MovieCollection.Add(movie);
            await _context.SaveChangesAsync();
        }

        // READ ALL
        public async Task<List<MovieDto>> GetUserMoviesAsync(int userId)
        {
            return await _context.MovieCollection
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Barcode = m.Barcode,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync();
        }

        // READ ONE
        public async Task<MovieDto?> GetMovieByIdAsync(int userId, int movieId)
        {
            return await _context.MovieCollection
                .Where(m => m.UserId == userId && m.Id == movieId)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Barcode = m.Barcode,
                    CreatedAt = m.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        // UPDATE
        public async Task<bool> UpdateMovieAsync(int userId, int movieId, string newBarcode)
        {
            ValidateBarcode(newBarcode);

            var movie = await _context.MovieCollection
                .FirstOrDefaultAsync(m => m.Id == movieId && m.UserId == userId);

            if (movie == null) return false;

            movie.Barcode = newBarcode;
            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> DeleteMovieAsync(int userId, int movieId)
        {
            var movie = await _context.MovieCollection
                .FirstOrDefaultAsync(m => m.Id == movieId && m.UserId == userId);

            if (movie == null) return false;

            _context.MovieCollection.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}