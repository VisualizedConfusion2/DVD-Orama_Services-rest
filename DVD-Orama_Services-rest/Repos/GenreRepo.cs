using DVD_Orama_Services_rest.Data;
using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVD_Orama_Services_rest.Repos
{
    public class GenreRepo : IGenreRepo
    {
        private readonly AppDbContext _context;

        public GenreRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GenreDto>> GetAllGenresAsync()
        {
            return await _context.Genres
                .Select(g => new GenreDto
                {
                    Id = g.GenreId,
                    Name = g.GenreName
                })
                .ToListAsync();
        }
    }
}
