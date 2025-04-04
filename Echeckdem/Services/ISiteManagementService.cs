using Echeckdem.CustomFolder;

namespace Echeckdem.Services
{
    public interface ISiteManagementService
    {
        Task<List<OrganisationsListViewModel>> GetActiveOrganizationsAsync();
        Task<List<LocationViewModel>> GetLocationsByOidAsync(string oid);
    }
}
