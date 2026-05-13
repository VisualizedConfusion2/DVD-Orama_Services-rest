using DVD_Orama_Services_rest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVD_Orama_Services_rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StreamingServiceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StreamingServiceController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/streamingservice
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllStreamingServices()
        {
            var services = await _context.StreamingLocations
                .Select(s => s.StreamingServiceName)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            return Ok(services);
        }
    }
}
