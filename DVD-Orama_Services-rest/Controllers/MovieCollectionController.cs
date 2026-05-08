using DVD_Orama_Services_rest.Models;
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

        // ─── Read ──────────────────────────────────────────────────────────────────

        // GET api/moviecollection?userId=1
        // Returns all collections the user is a member of + all public collections
        [HttpGet]
        public async Task<ActionResult<List<MovieCollectionDto>>> GetAllCollections([FromQuery] int userId)
        {
            var collections = await _collectionRepo.GetAllCollectionsAsync(userId);
            return Ok(collections);
        }

        // GET api/moviecollection/{id}?userId=1
        // Returns the collection if it is public, or the user has any role in it
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieCollectionDto>> GetCollectionById(int id, [FromQuery] int userId)
        {
            var collection = await _collectionRepo.GetCollectionByIdAsync(userId, id);
            if (collection == null) return NotFound();
            return Ok(collection);
        }

        // ─── Create ────────────────────────────────────────────────────────────────

        // POST api/moviecollection?userId=1
        // Creates a new collection; the caller becomes Owner (RoleId = 1)
        [HttpPost]
        public async Task<ActionResult> CreateCollection(
            [FromQuery] int userId,
            [FromBody] CreateMovieCollectionDto dto)
        {
            try
            {
                var id = await _collectionRepo.CreateCollectionAsync(userId, dto);
                return Ok(new { message = "Collection created successfully.", collectionId = id });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the collection.");
            }
        }

        // ─── Update ────────────────────────────────────────────────────────────────

        // PUT api/moviecollection/{id}?userId=1
        // Requires Owner (1) or Co-Owner (2)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollection(
            int id,
            [FromQuery] int userId,
            [FromBody] CreateMovieCollectionDto dto)
        {
            var updated = await _collectionRepo.UpdateCollectionAsync(userId, id, dto);
            if (!updated) return Forbid(); // Either not found or insufficient role
            return Ok(new { message = "Collection updated successfully." });
        }

        // ─── Delete ────────────────────────────────────────────────────────────────

        // DELETE api/moviecollection/{id}?userId=1
        // Requires Owner (1) only
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id, [FromQuery] int userId)
        {
            var deleted = await _collectionRepo.DeleteCollectionAsync(userId, id);
            if (!deleted) return Forbid(); // Either not found or not the Owner
            return NoContent();
        }

        // ─── Movie management ──────────────────────────────────────────────────────

        // PUT api/moviecollection/{id}/movies/{movieId}?userId=1
        // Requires Owner (1), Co-Owner (2), or Admin (3)
        [HttpPut("{id}/movies/{movieId}")]
        public async Task<IActionResult> AddMovieToCollection(
            int id,
            int movieId,
            [FromQuery] int userId)
        {
            var added = await _collectionRepo.AddMovieToCollectionAsync(userId, id, movieId);
            if (!added) return Forbid(); // Insufficient role, or movie already in collection
            return Ok(new { message = "Movie added to collection successfully." });
        }

        // DELETE api/moviecollection/{id}/movies/{movieId}?userId=1
        // Requires Owner (1), Co-Owner (2), or Admin (3)
        [HttpDelete("{id}/movies/{movieId}")]
        public async Task<IActionResult> RemoveMovieFromCollection(
            int id,
            int movieId,
            [FromQuery] int userId)
        {
            var removed = await _collectionRepo.RemoveMovieFromCollectionAsync(userId, id, movieId);
            if (!removed) return Forbid(); // Insufficient role, or movie not in collection
            return NoContent();
        }

        // ─── Invite ────────────────────────────────────────────────────────────────

        // POST api/moviecollection/{id}/invite?userId=1
        // Body: { "targetUserId": 5, "roleId": 3 }
        //
        // Role assignment rules:
        //   Owner (1)    → can assign Co-Owner (2), Admin (3), Viewer (4)
        //   Co-Owner (2) → can assign Admin (3), Viewer (4)
        //   Admin (3)    → can assign Viewer (4) only
        //   Viewer (4)   → cannot invite anyone
        //
        // If target already has the same role   → 409 Conflict
        // If target already has a different role → 200 Updated
        // If target is new to the collection    → 200 Success
        [HttpPost("{id}/invite")]
        public async Task<IActionResult> InviteUser(
            int id,
            [FromQuery] int userId,
            [FromBody] InviteUserDto dto)
        {
            var result = await _collectionRepo.InviteUserToCollectionAsync(userId, id, dto.TargetUserId, dto.RoleId);

            return result switch
            {
                InviteResult.Success =>
                    Ok(new { message = "User successfully invited to the collection." }),

                InviteResult.Updated =>
                    Ok(new { message = "User's role in the collection has been updated." }),

                InviteResult.SameRoleConflict =>
                    Conflict(new { message = "This user is already assigned this role in the collection." }),

                InviteResult.NotAuthorized =>
                    Forbid(),

                InviteResult.CannotAssignHigherRole =>
                    BadRequest(new { message = "You cannot assign a role equal to or higher than your own." }),

                InviteResult.TargetNotFound =>
                    NotFound(new { message = "The target user does not exist." }),

                InviteResult.CollectionNotFound =>
                    NotFound(new { message = "The collection does not exist." }),

                _ => StatusCode(500, "An unexpected error occurred.")
            };
        }
    }
}