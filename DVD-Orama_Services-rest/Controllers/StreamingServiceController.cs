using DVD_Orama_Services_rest.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StreamingServiceController : ControllerBase
{
    private readonly IStreamingServiceRepo _streamingServiceRepo;

    public StreamingServiceController(IStreamingServiceRepo streamingServiceRepo)
    {
        _streamingServiceRepo = streamingServiceRepo;
    }

    [HttpGet]
    public async Task<ActionResult<List<string>>> GetAllStreamingServices()
    {
        var services = await _streamingServiceRepo.GetAllStreamingServicesAsync();
        return Ok(services);
    }
}