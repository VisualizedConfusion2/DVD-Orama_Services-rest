using DVD_Orama_Services_rest.Data;
using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Models.Entities;
using DVD_Orama_Services_rest.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVD_Orama_Services_rest.Repos
{
    public class MovieRepo : IMovieRepo
    {
        private readonly AppDbContext _context;

        public MovieRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieDto>> GetAllMoviesAsync()
        {
            return await _context.Movies
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    PosterUrl = m.PosterUrl,
                    PublicationYear = m.PublicationYear
                })
                .ToListAsync();
        }

        public async Task<MovieDto?> GetMovieByIdAsync(int movieId)
        {
            return await _context.Movies
                .Where(m => m.Id == movieId)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    PosterUrl = m.PosterUrl,
                    PublicationYear = m.PublicationYear
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateMovieAsync(CreateMovieDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
                Duration = dto.Duration,
                PublicationYear = dto.PublicationYear,
                PosterUrl = dto.PosterUrl
            };
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie.Id;
        }

        public async Task<bool> UpdateMovieAsync(int movieId, CreateMovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return false;

            movie.Title = dto.Title;
            movie.Description = dto.Description;
            movie.Duration = dto.Duration;
            movie.PublicationYear = dto.PublicationYear;
            movie.PosterUrl = dto.PosterUrl;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMovieAsync(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}