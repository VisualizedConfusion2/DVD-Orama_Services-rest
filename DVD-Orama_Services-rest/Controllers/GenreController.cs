namespace DVD_Orama_Services_rest.Controllers
{
    using global::DVD_Orama_Services_rest.Data;
    using global::DVD_Orama_Services_rest.Models.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace DVD_Orama_Services_rest.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class GenreController : ControllerBase
        {
            private readonly AppDbContext _context;

            public GenreController(AppDbContext context)
            {
                _context = context;
            }

            // GET api/genre
            [HttpGet]
            public async Task<ActionResult<List<GenreDto>>> GetAllGenres()
            {
                var genres = await _context.Genres
                    .Select(g => new GenreDto
                    {
                        Id = g.GenreId,
                        Name = g.GenreName
                    })
                    .ToListAsync();

                return Ok(genres);
            }
        }
    }
}
