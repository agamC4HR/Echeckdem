using Echeckdem.Models;
using Echeckdem.CustomFolder;

namespace Echeckdem.Handlers
{
    public interface IJwtService
    {
        string GenerateJwtToken(LoginViewModel model);
    }
}
