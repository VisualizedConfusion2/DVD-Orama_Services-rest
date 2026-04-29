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

        [HttpGet]
        public async Task<ActionResult<List<MovieDto>>> GetMyMovies()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var movies = await _movieService.GetUserMoviesAsync(userId.Value);
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] AddMovieDto movieDto)
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

        /// <summary>
        /// Helper to extract the User ID from the JWT/Authentication claims
        /// </summary>
        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int id))
            {
                return id;
            }
            return null;
        }
    }
}