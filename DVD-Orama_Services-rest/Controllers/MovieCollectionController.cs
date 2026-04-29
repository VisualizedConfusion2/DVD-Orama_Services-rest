using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DVD_Orama_Services_rest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MovieCollectionController : ControllerBase
    {
        private readonly IMovieCollectionService _movieService;

        public MovieCollectionController(IMovieCollectionService movieService)
        {
            _movieService = movieService;
        }

        // GET api/moviecollection
        [HttpGet]
        public async Task<ActionResult<List<MovieDto>>> GetMyMovies()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var movies = await _movieService.GetUserMoviesAsync(userId.Value);
            return Ok(movies);
        }

        // GET api/moviecollection/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovieById(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var movie = await _movieService.GetMovieByIdAsync(userId.Value, id);
            if (movie == null) return NotFound();

            return Ok(movie);
        }

        // POST api/moviecollection
        [HttpPost]
        public async Task<ActionResult<MovieDto>> AddMovie([FromBody] AddMovieDto movieDto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                await _movieService.AddMovieAsync(userId.Value, movieDto.Barcode);
                return Ok(new { message = "Movie added successfully to your collection." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding the movie.");
            }
        }

        // PUT api/moviecollection/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] UpdateMovieDto movieDto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var updated = await _movieService.UpdateMovieAsync(userId.Value, id, movieDto.Barcode);
                if (!updated) return NotFound();

                return Ok(new { message = "Movie updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the movie.");
            }
        }

        // DELETE api/moviecollection/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var deleted = await _movieService.DeleteMovieAsync(userId.Value, id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int id))
                return id;
            return null;
        }
    }
}