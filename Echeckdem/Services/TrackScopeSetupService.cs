using Echeckdem.Models;
using System.Collections.Generic;
using System.Linq;

namespace Echeckdem.Services
{
    public class TrackScopeSetupService : ITrackScopeSetupService
    {
        private readonly DbEcheckContext _EcheckContext;

        public TrackScopeSetupService(DbEcheckContext EcheckContext)
        {
            _EcheckContext = EcheckContext;
        }


        public void AddTrackScope(TrackScope trackScope)
        {
            _EcheckContext.TrackScopes.Add(trackScope);
            _EcheckContext.SaveChanges();
        }



    }
}
