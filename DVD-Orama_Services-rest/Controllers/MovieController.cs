using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DVD_Orama_Services_rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepo _movieRepo;

        public MovieController(IMovieRepo movieRepo)
        {
            _movieRepo = movieRepo;
        }

        // GET api/movie
        [HttpGet]
        public async Task<ActionResult<List<MovieDto>>> GetAllMovies()
        {
            var movies = await _movieRepo.GetAllMoviesAsync();
            return Ok(movies);
        }

        // GET api/movie/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovieById(int id)
        {
            var movie = await _movieRepo.GetMovieByIdAsync(id);
            if (movie == null) return NotFound();
            return Ok(movie);
        }

        // POST api/movie
        [HttpPost]
        public async Task<ActionResult> CreateMovie([FromBody] CreateMovieDto dto)
        {
            try
            {
                var id = await _movieRepo.CreateMovieAsync(dto);
                return Ok(new { message = "Movie created successfully.", movieId = id });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the movie.");
            }
        }

        // PUT api/movie/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] CreateMovieDto dto)
        {
            var updated = await _movieRepo.UpdateMovieAsync(id, dto);
            if (!updated) return NotFound();
            return Ok(new { message = "Movie updated successfully." });
        }

        // DELETE api/movie/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var deleted = await _movieRepo.DeleteMovieAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        
    }
}