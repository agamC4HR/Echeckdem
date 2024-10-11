using Echeckdem.Models;
using Echeckdem.CustomFolder;

namespace Echeckdem.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(LoginViewModel model);
    }
}
