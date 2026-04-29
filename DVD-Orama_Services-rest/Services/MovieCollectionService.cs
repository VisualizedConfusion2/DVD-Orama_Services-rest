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

        public async Task AddMovieAsync(int userId, string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                throw new ArgumentException("Barcode cannot be empty");

            // Simpel EAN/UPC validering (MVP)
            if (barcode.Length < 8 || barcode.Length > 14)
                throw new ArgumentException("Invalid barcode format");

            var movie = new MovieCollectionItem
            {
                Barcode = barcode,
                UserId = userId
            };

            _context.MovieCollection.Add(movie);
            await _context.SaveChangesAsync();
        }

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
    }
}
