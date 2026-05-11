using DVD_Orama_Services_rest.Data;
using DVD_Orama_Services_rest.Models;
using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Models.Entities;
using DVD_Orama_Services_rest.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVD_Orama_Services_rest.Repos
{
    // Role IDs as defined in the database seed:
    // 1 = Owner    → full control including delete
    // 2 = Co-Owner → invite, add/remove/edit movies, cannot delete
    // 3 = Admin    → add/remove/edit movies only
    // 4 = Viewer   → read-only

    public class MovieCollectionRepo : IMovieCollectionRepo
    {
        private readonly AppDbContext _context;

        public MovieCollectionRepo(AppDbContext context)
        {
            _context = context;
        }

        // ─── Role helper ───────────────────────────────────────────────────────────

        /// <summary>
        /// Returns the RoleId the user holds in the given collection, or null if they have no mapping.
        /// </summary>
        public async Task<int?> GetUserRoleAsync(int userId, int collectionId)
        {
            var map = await _context.UserMovieCollectionMap
                .FirstOrDefaultAsync(u => u.UserId == userId && u.MovieCollectionId == collectionId);
            return map?.RoleId;
        }

        // ─── Read ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns all collections the user can view:
        ///   • All collections they have any role in (Owner / Co-Owner / Admin / Viewer)
        ///   • All public collections (IsPublic = true), regardless of membership
        /// </summary>
        public async Task<List<MovieCollectionDto>> GetAllCollectionsAsync(int userId)
        {
            // IDs of collections the user is a member of
            var memberCollectionIds = await _context.UserMovieCollectionMap
                .Where(u => u.UserId == userId)
                .Select(u => u.MovieCollectionId)
                .ToListAsync();

            return await _context.MovieCollections
                .Where(c => c.IsPublic || memberCollectionIds.Contains(c.Id))
                .Select(c => new MovieCollectionDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsPublic = c.IsPublic,
                    Movies = _context.MovieCollectionsMoviesMap
                        .Where(m => m.MovieCollectionId == c.Id)
                        .Join(_context.Movies,
                            m => m.MovieId,
                            movie => movie.MovieId,
                            (m, movie) => new MovieDto
                            {
                                MovieId = movie.MovieId,
                                Title = movie.Title,
                                PosterUrl = movie.PosterUrl,
                                PublicationYear = movie.PublicationYear
                            })
                        .ToList()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Returns a single collection if:
        ///   • The collection is public (IsPublic = true), OR
        ///   • The user has any role in the collection
        /// </summary>
        public async Task<MovieCollectionDto?> GetCollectionByIdAsync(int userId, int collectionId)
        {
            var collection = await _context.MovieCollections
                .FirstOrDefaultAsync(c => c.Id == collectionId);

            if (collection == null) return null;

            // Allow access if public, or user has any membership
            if (!collection.IsPublic)
            {
                var role = await GetUserRoleAsync(userId, collectionId);
                if (role == null) return null;
            }

            return await _context.MovieCollections
                .Where(c => c.Id == collectionId)
                .Select(c => new MovieCollectionDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsPublic = c.IsPublic,
                    Movies = _context.MovieCollectionsMoviesMap
                        .Where(m => m.MovieCollectionId == c.Id)
                        .Join(_context.Movies,
                            m => m.MovieId,
                            movie => movie.MovieId,
                            (m, movie) => new MovieDto
                            {
                                MovieId = movie.MovieId,
                                Title = movie.Title,
                                PosterUrl = movie.PosterUrl,
                                PublicationYear = movie.PublicationYear
                            })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        // ─── Create ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Creates a new collection and assigns the creator as Owner (RoleId = 1).
        /// Any authenticated user can create a collection.
        /// </summary>
        public async Task<int> CreateCollectionAsync(int userId, CreateMovieCollectionDto dto)
        {
            var collection = new MovieCollection
            {
                Name = dto.Name,
                IsPublic = dto.IsPublic
            };
            _context.MovieCollections.Add(collection);
            await _context.SaveChangesAsync();

            _context.UserMovieCollectionMap.Add(new UserMovieCollectionMap
            {
                UserId = userId,
                MovieCollectionId = collection.Id,
                RoleId = 1 // Owner
            });
            await _context.SaveChangesAsync();
            return collection.Id;
        }

        // ─── Update ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Updates collection name / visibility.
        /// Requires: Owner (1) or Co-Owner (2).
        /// </summary>
        public async Task<bool> UpdateCollectionAsync(int userId, int collectionId, CreateMovieCollectionDto dto)
        {
            var role = await GetUserRoleAsync(userId, collectionId);
            if (role == null || role > 2) return false; // Must be Owner or Co-Owner

            var collection = await _context.MovieCollections.FindAsync(collectionId);
            if (collection == null) return false;

            collection.Name = dto.Name;
            collection.IsPublic = dto.IsPublic;
            await _context.SaveChangesAsync();
            return true;
        }

        // ─── Delete ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Deletes the entire collection and all associated data.
        /// Requires: Owner (1) only.
        /// </summary>
        public async Task<bool> DeleteCollectionAsync(int userId, int collectionId)
        {
            var role = await GetUserRoleAsync(userId, collectionId);
            if (role != 1) return false; // Owner only

            var collection = await _context.MovieCollections.FindAsync(collectionId);
            if (collection == null) return false;

            // 1. Remove all movie-collection mappings
            var movieMappings = _context.MovieCollectionsMoviesMap
                .Where(m => m.MovieCollectionId == collectionId);
            _context.MovieCollectionsMoviesMap.RemoveRange(movieMappings);

            // 2. Remove all user-collection mappings (all members)
            var userMappings = _context.UserMovieCollectionMap
                .Where(u => u.MovieCollectionId == collectionId);
            _context.UserMovieCollectionMap.RemoveRange(userMappings);

            await _context.SaveChangesAsync();

            // 3. Delete the collection itself
            _context.MovieCollections.Remove(collection);
            await _context.SaveChangesAsync();

            return true;
        }

        // ─── Movie management ──────────────────────────────────────────────────────

        /// <summary>
        /// Adds a movie to the collection.
        /// Requires: Owner (1), Co-Owner (2), or Admin (3).
        /// </summary>
        public async Task<bool> AddMovieToCollectionAsync(int userId, int collectionId, int movieId)
        {
            var role = await GetUserRoleAsync(userId, collectionId);
            if (role == null || role > 3) return false; // Must be Owner, Co-Owner, or Admin

            var exists = await _context.MovieCollectionsMoviesMap
                .AnyAsync(m => m.MovieCollectionId == collectionId && m.MovieId == movieId);
            if (exists) return false;

            _context.MovieCollectionsMoviesMap.Add(new MovieCollectionsMoviesMap
            {
                MovieCollectionId = collectionId,
                MovieId = movieId
            });
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Removes a movie from the collection.
        /// Requires: Owner (1), Co-Owner (2), or Admin (3).
        /// </summary>
        public async Task<bool> RemoveMovieFromCollectionAsync(int userId, int collectionId, int movieId)
        {
            var role = await GetUserRoleAsync(userId, collectionId);
            if (role == null || role > 3) return false; // Must be Owner, Co-Owner, or Admin

            var entry = await _context.MovieCollectionsMoviesMap
                .FirstOrDefaultAsync(m => m.MovieCollectionId == collectionId && m.MovieId == movieId);
            if (entry == null) return false;

            _context.MovieCollectionsMoviesMap.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }

        // ─── Invite ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Invites a user to a collection with a given role, or updates their role if they are already a member.
        ///
        /// Role assignment rules:
        ///   Owner (1)    → can assign Co-Owner (2), Admin (3), Viewer (4)
        ///   Co-Owner (2) → can assign Admin (3), Viewer (4)
        ///   Admin (3)    → can assign Viewer (4) only
        ///   Viewer (4)   → cannot assign anyone
        ///
        /// If the target user already has the SAME role → SameRoleConflict
        /// If the target user has a DIFFERENT role     → Updated
        /// If the target user is new                   → Success
        /// </summary>
        public async Task<InviteResult> InviteUserToCollectionAsync(
            int inviterId, int collectionId, int targetUserId, int roleId)
        {
            // 1. Verify collection exists
            var collectionExists = await _context.MovieCollections.AnyAsync(c => c.Id == collectionId);
            if (!collectionExists) return InviteResult.CollectionNotFound;

            // 2. Verify inviter has a role in the collection
            var inviterRole = await GetUserRoleAsync(inviterId, collectionId);
            if (inviterRole == null) return InviteResult.NotAuthorized;

            // 3. Viewers cannot invite anyone
            if (inviterRole == 4) return InviteResult.NotAuthorized;

            // 4. Inviter cannot assign a role equal to or higher than their own
            //    (lower RoleId = higher privilege, so roleId must be > inviterRole)
            if (roleId <= inviterRole) return InviteResult.CannotAssignHigherRole;

            // 5. Validate that the target roleId is a valid assignable role (2–4)
            //    RoleId 1 (Owner) can never be assigned via invite
            if (roleId < 2 || roleId > 4) return InviteResult.CannotAssignHigherRole;

            // 6. Verify target user exists
            var targetExists = await _context.Users.AnyAsync(u => u.Id == targetUserId);
            if (!targetExists) return InviteResult.TargetNotFound;

            // 7. Check if target already has a role in this collection
            var existingMap = await _context.UserMovieCollectionMap
                .FirstOrDefaultAsync(u => u.UserId == targetUserId && u.MovieCollectionId == collectionId);

            if (existingMap != null)
            {
                // Same role → conflict
                if (existingMap.RoleId == roleId) return InviteResult.SameRoleConflict;

                // Different role → update
                existingMap.RoleId = roleId;
                await _context.SaveChangesAsync();
                return InviteResult.Updated;
            }

            // 8. New membership → add
            _context.UserMovieCollectionMap.Add(new UserMovieCollectionMap
            {
                UserId = targetUserId,
                MovieCollectionId = collectionId,
                RoleId = roleId
            });
            await _context.SaveChangesAsync();
            return InviteResult.Success;
        }
    }
}