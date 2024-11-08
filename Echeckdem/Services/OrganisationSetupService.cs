using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Services
{
    public class OrganisationSetupService
    {
        private readonly DbEcheckContext _EcheckContext;

        public OrganisationSetupService(DbEcheckContext EcheckContext)  
        {
            _EcheckContext = EcheckContext;
        }

        
        public async Task<List<OrganisationListViewModel>> GetActiveOrganisationsListAsync(string searchTerm)       // organisation List
        {
            var query = _EcheckContext.Ncmorgs
                .Where(ncm => ncm.Oactive == 1);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(ncm => ncm.Oname.Contains(searchTerm));
            }

            return await query
                .Select(ncm => new OrganisationListViewModel
                {
                    Oname = ncm.Oname,
                    oid = ncm.Oid
                })
                .ToListAsync();
        }
        
        public async Task<OrganisationGeneralInfoViewModel?> GetOrganisationGeneralInformationAsync(string oid)     // General Information 
        {
            if (string.IsNullOrEmpty(oid))
                throw new ArgumentException("Organisation ID cannot be null or empty", nameof(oid));

            return await _EcheckContext.Ncmorgs
                .Where(o => o.Oid == oid)
                .Select(o => new OrganisationGeneralInfoViewModel
                {
                    Oname = o.Oname,
                    oid = o.Oid,
                    Spoc = o.Spoc,
                    styear = o.Styear,
                    Contname = o.Contname,
                    Contemail = o.Contemail
                })
                .FirstOrDefaultAsync();
        }


        
    }
}
