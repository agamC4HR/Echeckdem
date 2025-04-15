using Echeckdem.CustomFolder;

namespace Echeckdem.Services
{
    public interface ISiteManagementService
    {
        Task<List<OrganisationsListViewModel>> GetActiveOrganizationsAsync();
        Task<List<LocationViewModel>> GetLocationsByOidAsync(string oid);

        Task<LocationViewModel?> GetLocationDetailsAsync(string oid, string lcode);
        Task<List<ReturnTemplateViewModel>> GetApplicableReturnsAsync(ReturnPeriodSelectionViewModel input);
        Task SaveSelectedReturnsAsync(ReturnPeriodSelectionViewModel input);


    }

}
