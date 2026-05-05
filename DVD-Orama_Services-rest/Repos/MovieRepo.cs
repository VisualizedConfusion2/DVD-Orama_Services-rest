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
            var movies = await _context.Movies
                .Select(m => new MovieDto
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Description = m.Description,
                    Duration = m.Duration,
                    PublicationYear = m.PublicationYear,
                    PosterUrl = m.PosterUrl,
                    Actors = _context.MoviesActorsMap
                        .Where(map => map.MovieId == m.MovieId)
                        .Join(_context.Actors,
                            map => map.ActorId,
                            actor => actor.ActorId,
                            (map, actor) => actor.Name)
                        .ToList(),
                    Genres = _context.MoviesGenresMap
                        .Where(map => map.MovieId == m.MovieId)
                        .Join(_context.Genres,
                            map => map.GenreId,
                            genre => genre.GenreId,
                            (map, genre) => genre.GenreName)
                        .ToList(),
                    EANs = _context.MovieEAN
                        .Where(e => e.MovieId == m.MovieId)
                        .Select(e => e.EAN)
                        .ToList(),
                    StreamingLocations = _context.StreamingLocations
                        .Where(s => s.MovieId == m.MovieId)
                        .Select(s => new StreamingLocation
                        {
                            StreamingServiceId = s.StreamingServiceId,
                            StreamingServiceName = s.StreamingServiceName,
                            URL = s.URL
                        })
                        .ToList()
                })
                .ToListAsync();

            return movies;
        }

        public async Task<MovieDto?> GetMovieByIdAsync(int movieId)
        {
            return await _context.Movies
                .Where(m => m.MovieId == movieId)
                .Select(m => new MovieDto
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Description = m.Description,
                    Duration = m.Duration,
                    PublicationYear = m.PublicationYear,
                    PosterUrl = m.PosterUrl,
                    Actors = _context.MoviesActorsMap
                        .Where(map => map.MovieId == m.MovieId)
                        .Join(_context.Actors,
                            map => map.ActorId,
                            actor => actor.ActorId,
                            (map, actor) => actor.Name)
                        .ToList(),
                    Genres = _context.MoviesGenresMap
                        .Where(map => map.MovieId == m.MovieId)
                        .Join(_context.Genres,
                            map => map.GenreId,
                            genre => genre.GenreId,
                            (map, genre) => genre.GenreName)
                        .ToList(),
                    EANs = _context.MovieEAN
                        .Where(e => e.MovieId == m.MovieId)
                        .Select(e => e.EAN)
                        .ToList(),
                    StreamingLocations = _context.StreamingLocations
                        .Where(s => s.MovieId == m.MovieId)
                        .Select(s => new StreamingLocation
                        {
                            StreamingServiceId = s.StreamingServiceId,
                            StreamingServiceName = s.StreamingServiceName,
                            URL = s.URL
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateMovieAsync(CreateMovieDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Create Movie
                var movie = new Movie
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Duration = dto.Duration,
                    PublicationYear = dto.PublicationYear,
                    PosterUrl = dto.PosterUrl
                };

                _context.Movies.Add(movie);
                await _context.SaveChangesAsync(); // ensures MovieId is generated

                // 2. ACTORS
                foreach (var actorName in dto.Actors.Distinct())
                {
                    var actor = await _context.Actors
                        .FirstOrDefaultAsync(a => a.Name == actorName);

                    if (actor == null)
                    {
                        actor = new Actor { Name = actorName };
                        _context.Actors.Add(actor);
                        await _context.SaveChangesAsync();
                    }

                    _context.MoviesActorsMap.Add(new MoviesActorsMap
                    {
                        MovieId = movie.MovieId,
                        ActorId = actor.ActorId
                    });
                }

                // 3. GENRES
                foreach (var genreName in dto.Genres.Distinct())
                {
                    var genre = await _context.Genres
                        .FirstOrDefaultAsync(g => g.GenreName == genreName);

                    if (genre == null)
                    {
                        genre = new Genre { GenreName = genreName };
                        _context.Genres.Add(genre);
                        await _context.SaveChangesAsync();
                    }

                    _context.MoviesGenresMap.Add(new MoviesGenresMap
                    {
                        MovieId = movie.MovieId,
                        GenreId = genre.GenreId
                    });
                }

                // 4. EANs
                foreach (var ean in dto.EANs.Distinct())
                {
                    _context.MovieEAN.Add(new MovieEAN
                    {
                        MovieId = movie.MovieId,
                        EAN = ean
                    });
                }

                // 5. Streaming Locations
                foreach (var s in dto.StreamingLocations)
                {
                    _context.StreamingLocations.Add(new StreamingLocation
                    {
                        MovieId = movie.MovieId,
                        StreamingServiceName = s.StreamingServiceName,
                        URL = s.URL
                    });
                }

                // 6. Save everything
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return movie.MovieId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to create movie", ex);
            }
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