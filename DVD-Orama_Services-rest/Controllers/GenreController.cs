using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GenreController : ControllerBase
{
    private readonly IGenreRepo _genreRepo;

    public GenreController(IGenreRepo genreRepo)
    {
        _genreRepo = genreRepo;
    }

    [HttpGet]
    public async Task<ActionResult<List<GenreDto>>> GetAllGenres()
    {
        var genres = await _genreRepo.GetAllGenresAsync();
        return Ok(genres);
    }
}
