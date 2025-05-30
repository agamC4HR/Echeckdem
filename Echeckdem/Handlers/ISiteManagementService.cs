﻿using Echeckdem.CustomFolder;

namespace Echeckdem.Handlers
{
    public interface ISiteManagementService
    {
        Task<List<OrganisationsListViewModel>> GetActiveOrganizationsAsync();
        Task<List<LocationViewModel>> GetLocationsByOidAsync(string oid);

        Task<LocationViewModel?> GetLocationDetailsAsync(string oid, string lcode);
        Task<List<ReturnTemplateViewModel>> GetApplicableReturnsAsync(ReturnPeriodSelectionViewModel input);
        Task SaveSelectedReturnsAsync(ReturnPeriodSelectionViewModel input);

        Task<List<IGrouping<int, ReturnDetailViewModel>>> GetSubmittedReturnsByOrg(string oid, string lcode);

        Task<List<ContributionTemplateViewModel>> GetApplicableContributionsAsync(ContributionPeriodSelectionViewModel input);

        Task SaveSelectedContributionsAsync(ContributionPeriodSelectionViewModel input);

        Task<List<IGrouping<int, ContributionDetailViewModel>>> GetSubmittedContributionsByOrg(string oid, string lcode);

        Task<List<RegistrationTemplateViewModel>> GetApplicableRegistrationsAsync(string ltype, string lstate, string oid, string lcode);

        Task SaveSelectedRegistrationsAsync(RegistrationSelectionViewModel input);

        Task<List<RegistrationTemplateViewModel>> GetSubmittedRegistrationsAsync(string oid, string lcode);
    }

}
