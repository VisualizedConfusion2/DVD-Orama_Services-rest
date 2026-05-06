using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Repos.Interfaces;
using DVD_Orama_Services_rest.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepo _movieRepo;

    public MovieService(IMovieRepo movieRepo)
    {
        _movieRepo = movieRepo;
    }

    public async Task<int> UpsertMovieAsync(CreateMovieDto dto)
    {
        var searchDto = new MovieSearchDto
        {
            Title = dto.Title,
            PublicationYear = dto.PublicationYear
        };

        var existing = await _movieRepo.SearchMoviesAsync(searchDto);

        if (existing.Any())
        {
            var movieId = existing[0];
            await _movieRepo.UpdateMovieAsync(movieId, dto);
            return movieId;
        }

        return await _movieRepo.CreateMovieAsync(dto);
    }
}
