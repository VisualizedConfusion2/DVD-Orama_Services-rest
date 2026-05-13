namespace DVD_Orama_Services_rest.Repos.Interfaces
{
    public interface IStreamingServiceRepo
    {
        Task<List<string>> GetAllStreamingServicesAsync();
    }
}
