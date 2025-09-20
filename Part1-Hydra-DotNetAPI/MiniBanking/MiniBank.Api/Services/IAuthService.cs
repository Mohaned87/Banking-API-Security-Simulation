using static MiniBank.Api.Dtos.AuthDtos;

namespace MiniBank.Api.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
