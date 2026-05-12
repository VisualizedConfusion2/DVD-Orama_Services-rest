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
            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
                Duration = dto.Duration,
                PublicationYear = dto.PublicationYear,
                PosterUrl = dto.PosterUrl
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync(); // get MovieId

            await SyncMovieActorsAsync(movie.MovieId, dto.Actors);
            await SyncMovieGenresAsync(movie.MovieId, dto.Genres);
            await SyncMovieEansAsync(movie.MovieId, dto.EANs);
            await SyncMovieStreamingAsync(movie.MovieId, dto.StreamingLocations);

            await _context.SaveChangesAsync();

            return movie.MovieId;
        }

        public async Task<bool> UpdateMovieAsync(int movieId, CreateMovieDto dto)
        {
            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == movieId);

            if (movie == null)
                return false;

            if (!string.IsNullOrWhiteSpace(dto.Title))
                movie.Title = dto.Title;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                movie.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.PosterUrl))
                movie.PosterUrl = dto.PosterUrl;

            if (dto.Duration > 0)
                movie.Duration = dto.Duration;

            if (dto.PublicationYear > 1888 )
                movie.PublicationYear = dto.PublicationYear;

            await SyncMovieActorsAsync(movieId, dto.Actors);

            await SyncMovieGenresAsync(movieId, dto.Genres);

            await SyncMovieEansAsync(movieId, dto.EANs);

            await SyncMovieStreamingAsync(movieId, dto.StreamingLocations);

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
        public async Task<List<MovieDto>> SearchMoviesAsync(MovieSearchDto dto)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Title))
                query = query.Where(m => m.Title.Contains(dto.Title));

            if (dto.PublicationYear.HasValue)
                query = query.Where(m => m.PublicationYear == dto.PublicationYear);

            if (dto.Genres?.Any() == true)
                query = query.Where(m =>
                    _context.MoviesGenresMap
                        .Where(x => x.MovieId == m.MovieId)
                        .Join(_context.Genres,
                            x => x.GenreId,
                            g => g.GenreId,
                            (x, g) => g.GenreName)
                        .Any(g => dto.Genres.Contains(g)));

            if (dto.Actors?.Any() == true)
                query = query.Where(m =>
                    _context.MoviesActorsMap
                        .Where(x => x.MovieId == m.MovieId)
                        .Join(_context.Actors,
                            x => x.ActorId,
                            a => a.ActorId,
                            (x, a) => a.Name)
                        .Any(a => dto.Actors.Contains(a)));

            if (dto.StreamingServices?.Any() == true)
                query = query.Where(m =>
                    _context.StreamingLocations
                        .Where(s => s.MovieId == m.MovieId)
                        .Any(s => dto.StreamingServices.Contains(s.StreamingServiceName)));

            // Only this part changes
            return await query
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
        }
        private async Task SyncMovieGenresAsync(int movieId, List<string>? genres)
        {
            if (genres == null)
                return;

            var clean = genres
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .Select(g => g.Trim())
                .Distinct()
                .ToList();

            foreach (var name in clean)
            {
                var genre = await _context.Genres
                    .FirstOrDefaultAsync(g => g.GenreName == name);

                if (genre == null)
                {
                    genre = new Genre { GenreName = name };
                    _context.Genres.Add(genre);
                    await _context.SaveChangesAsync();
                }

                var exists = await _context.MoviesGenresMap
                    .AnyAsync(x => x.MovieId == movieId && x.GenreId == genre.GenreId);

                if (!exists)
                {
                    _context.MoviesGenresMap.Add(new MoviesGenresMap
                    {
                        MovieId = movieId,
                        GenreId = genre.GenreId
                    });
                }
            }
        }
        private async Task SyncMovieEansAsync(int movieId, List<long>? eans)
        {
            if (eans == null)
                return;

            var clean = eans
                .Where(e => e > 0)
                .Distinct()
                .ToList();

            foreach (var ean in clean)
            {
                var exists = await _context.MovieEAN
                    .AnyAsync(x => x.MovieId == movieId && x.EAN == ean);

                if (!exists)
                {
                    _context.MovieEAN.Add(new MovieEAN
                    {
                        MovieId = movieId,
                        EAN = ean
                    });
                }
            }
        }
        private async Task SyncMovieStreamingAsync(int movieId, List<StreamingLocation>? streaming)
        {
            if (streaming == null)
                return;

            var clean = streaming
                .Where(s =>
                    s != null &&
                    !string.IsNullOrWhiteSpace(s.StreamingServiceName) &&
                    !string.IsNullOrWhiteSpace(s.URL))
                .Select(s => new StreamingLocation
                {
                    StreamingServiceName = s.StreamingServiceName.Trim(),
                    URL = s.URL.Trim()
                })
                .ToList();

            _context.StreamingLocations.RemoveRange(
                _context.StreamingLocations.Where(x => x.MovieId == movieId));

            _context.StreamingLocations.AddRange(clean.Select(s => new StreamingLocation
            {
                MovieId = movieId,
                StreamingServiceName = s.StreamingServiceName,
                URL = s.URL
            }));
        }
        private async Task SyncMovieActorsAsync(int movieId, List<string>? actors)
        {
            if (actors == null)
                return;

            var clean = actors
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(a => a.Trim())
                .Distinct()
                .ToList();

            foreach (var name in clean)
            {
                var actor = await _context.Actors
                    .FirstOrDefaultAsync(a => a.Name == name);

                if (actor == null)
                {
                    actor = new Actor { Name = name };
                    _context.Actors.Add(actor);
                    await _context.SaveChangesAsync();
                }

                var exists = await _context.MoviesActorsMap
                    .AnyAsync(x => x.MovieId == movieId && x.ActorId == actor.ActorId);

                if (!exists)
                {
                    _context.MoviesActorsMap.Add(new MoviesActorsMap
                    {
                        MovieId = movieId,
                        ActorId = actor.ActorId
                    });
                }
            }
        }
    }
}