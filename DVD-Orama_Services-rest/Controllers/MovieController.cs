using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Repos.Interfaces;
using DVD_Orama_Services_rest.Services;
using Microsoft.AspNetCore.Mvc;

namespace DVD_Orama_Services_rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepo _movieRepo;
        private readonly IMovieService _movieService;

        public MovieController(IMovieRepo movieRepo, IMovieService movieService)
        {
            _movieRepo = movieRepo;
            _movieService = movieService;
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
        [HttpGet("search")]
        public async Task<ActionResult<List<MovieDto>>> SearchMovies([FromQuery] MovieSearchDto dto)
        {
            var movies = await _movieRepo.SearchMoviesAsync(dto);

            if (!movies.Any())
                return NotFound(new { message = "No movies found matching the given criteria." });

            return Ok(movies);
        }

        // POST api/movie
        [HttpPost]
        public async Task<ActionResult> CreateMovie(CreateMovieDto dto)
        {
            try
            {
                var id = await _movieService.UpsertMovieAsync(dto);

                return Ok(new { movieId = id });
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