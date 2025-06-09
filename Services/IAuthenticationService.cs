using ArtMarketPlaceAPI.DTO;

namespace ArtMarketPlaceAPI.Services
{
    public interface IAuthenticationService
    {
        Task<string> RegisterUserAsync(Register registerDto);
        Task<object> LoginAsync(Login loginDto); 
    }
}
