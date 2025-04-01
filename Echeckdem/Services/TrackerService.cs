using Echeckdem.Models;

namespace Echeckdem.Services
{

    public class TrackerService
    {
        private readonly DbEcheckContext _dbEcheckContext;

        private TrackerService(DbEcheckContext dbEcheckContext)
        {
            _dbEcheckContext = dbEcheckContext;
        }


    }
}
