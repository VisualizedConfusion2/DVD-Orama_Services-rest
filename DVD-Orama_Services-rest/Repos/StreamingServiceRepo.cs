using DVD_Orama_Services_rest.Data;
using DVD_Orama_Services_rest.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVD_Orama_Services_rest.Repos
{
    public class StreamingServiceRepo : IStreamingServiceRepo
    {
        private readonly AppDbContext _context;

        public StreamingServiceRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetAllStreamingServicesAsync()
        {
            return await _context.StreamingLocations
                .Select(s => s.StreamingServiceName)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();
        }
    }
}
