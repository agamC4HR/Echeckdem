using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Services
{
    public class OrgListGeneralInformationService
    {
        private readonly DbEcheckContext _EcheckContext;

        public OrgListGeneralInformationService(DbEcheckContext EcheckContext)
        {
            _EcheckContext = EcheckContext;
        }

        public async Task<OrganisationList> GetOrganisationGeneralInformation(string oid)
        {
            if (string.IsNullOrEmpty(oid))
            {
                throw new ArgumentException("Organisation ID cannot be null or empty", nameof(oid));
            }

            return await _EcheckContext.Ncmorgs
                .Where(o => o.Oid == oid)  
                .Select(o => new OrganisationList
                {
                    oid = o.Oid,
                    spoc = o.Spoc
                })
                .FirstOrDefaultAsync();
        }
    }
}
