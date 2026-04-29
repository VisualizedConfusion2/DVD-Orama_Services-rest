using DVD_Orama_Services_rest.Models.DTOs;

namespace DVD_Orama_Services_rest.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}