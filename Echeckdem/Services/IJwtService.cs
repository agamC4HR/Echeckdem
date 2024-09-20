using Echeckdem.Models;

namespace Echeckdem.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(Users user);
    }
}
