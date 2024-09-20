using Echeckdem.Models;
using Echeckdem.CustomFolder;
using Microsoft.EntityFrameworkCore;


namespace Echeckdem.Services
{
    public class OrgListService
    {
        private readonly DbEcheckContext _EcheckContext;

        public OrgListService(DbEcheckContext EcheckContext)
        {
            _EcheckContext = EcheckContext;
        }

        // public async Task<List<OrganisationList>> GetActiveOrganisationsListAsync(string searchTerm)
        public async Task<List<OrganisationList>> GetActiveOrganisationsListAsync(string searchTerm)
        {
            // Define the base query
            var query = _EcheckContext.Ncmorgs
                .Where(ncm => ncm.Oactive == 1);  // Compare with 1 to get active records

            // Apply search filter if searchTerm is provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(ncm => ncm.Oname.Contains(searchTerm));
            }                       

            // Execute the query and map the results
            var ncmorgList = await query.ToListAsync();

            var organisationList = ncmorgList.Select(ncm => new OrganisationList
            {
                Oname = ncm.Oname,
                oid = ncm.Oid
            }).ToList();

            return organisationList;
        }

    }
}
