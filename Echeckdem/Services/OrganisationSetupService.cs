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

        public async Task<CombinedOrganisationSetupViewModel> GetOrganisationSetupAsync(string searchTerm, string? selectedOid)
        {
            // Fetch the list of active organizations
            var organisationsList = await _EcheckContext.Ncmorgs
                .Where(ncm => ncm.Oactive == 1 &&
                              (string.IsNullOrEmpty(searchTerm) || ncm.Oname.Contains(searchTerm)))
                .Select(ncm => new OrganisationsListViewModel
                {
                    Oname = ncm.Oname,
                    oid = ncm.Oid
                })
                .ToListAsync();

            // Fetch the selected organization's general information
            OrganisationGeneralInfoViewModel? selectedOrganisation = null;
            if (!string.IsNullOrEmpty(selectedOid))
            {
                selectedOrganisation = await _EcheckContext.Ncmorgs
                    .Where(o => o.Oid == selectedOid)
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

            return new CombinedOrganisationSetupViewModel
            {
                OrganisationsList = organisationsList,
                SelectedOrganisation = selectedOrganisation
            };
        }

        //public async Task<List<OrganisationsListViewModel>> GetActiveOrganisationsListAsync(string searchTerm)       // organisation List
        //{
        //    var query = _EcheckContext.Ncmorgs
        //        .Where(ncm => ncm.Oactive == 1);

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        query = query.Where(ncm => ncm.Oname.Contains(searchTerm));
        //    }

        //    return await query
        //        .Select(ncm => new OrganisationsListViewModel
        //        {
        //            Oname = ncm.Oname,
        //            oid = ncm.Oid
        //        })
        //        .ToListAsync();
        //}

        //public async Task<OrganisationGeneralInfoViewModel?> GetOrganisationGeneralInformationAsync(string oid)     // General Information 
        //{
        //    if (string.IsNullOrEmpty(oid))
        //        throw new ArgumentException("Organisation ID cannot be null or empty", nameof(oid));

        //    return await _EcheckContext.Ncmorgs
        //        .Where(o => o.Oid == oid)
        //        .Select(o => new OrganisationGeneralInfoViewModel
        //        {
        //            Oname = o.Oname,
        //            oid = o.Oid,
        //            Spoc = o.Spoc,
        //            styear = o.Styear,
        //            Contname = o.Contname,
        //            Contemail = o.Contemail
        //        })
        //        .FirstOrDefaultAsync();
        //}



    }
}
