using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DVD_Orama_Services_rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieCollectionController : ControllerBase
    {
        private readonly IMovieCollectionRepo _collectionRepo;

        public MovieCollectionController(IMovieCollectionRepo collectionRepo)
        {
            _collectionRepo = collectionRepo;
        }

        // GET api/moviecollection
        [HttpGet]
        public async Task<ActionResult<List<MovieCollectionDto>>> GetAllCollections()
        {
            var collections = await _collectionRepo.GetAllCollectionsAsync(1);
            return Ok(collections);
        }

        // GET api/moviecollection/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieCollectionDto>> GetCollectionById(int id)
        {
            var collection = await _collectionRepo.GetCollectionByIdAsync(1, id);
            if (collection == null) return NotFound();
            return Ok(collection);
        }

        // POST api/moviecollection
        [HttpPost]
        public async Task<ActionResult> CreateCollection([FromBody] CreateMovieCollectionDto dto)
        {
            try
            {
                var id = await _collectionRepo.CreateCollectionAsync(1, dto);
                return Ok(new { message = "Collection created successfully.", collectionId = id });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the collection.");
            }
        }

        // PUT api/moviecollection/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollection(int id, [FromBody] CreateMovieCollectionDto dto)
        {
            var updated = await _collectionRepo.UpdateCollectionAsync(1, id, dto);
            if (!updated) return NotFound();
            return Ok(new { message = "Collection updated successfully." });
        }

        // DELETE api/moviecollection/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            var deleted = await _collectionRepo.DeleteCollectionAsync(1, id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // PUT api/moviecollection/{id}/movies/{movieId}
        [HttpPut("{id}/movies/{movieId}")]
        public async Task<IActionResult> AddMovieToCollection(int id, int movieId)
        {
            var added = await _collectionRepo.AddMovieToCollectionAsync(1, id, movieId);
            if (!added) return NotFound();
            return Ok(new { message = "Movie added to collection successfully." });
        }

        // DELETE api/moviecollection/{id}/movies/{movieId}
        [HttpDelete("{id}/movies/{movieId}")]
        public async Task<IActionResult> RemoveMovieFromCollection(int id, int movieId)
        {
            var removed = await _collectionRepo.RemoveMovieFromCollectionAsync(1, id, movieId);
            if (!removed) return NotFound();
            return NoContent();
        }
    }
}