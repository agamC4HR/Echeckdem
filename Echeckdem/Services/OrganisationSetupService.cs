﻿using Echeckdem.CustomFolder;
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

        public async Task<bool> AddOrganisationAsync(OrganisationGeneralInfoViewModel newOrganisation)                        // Adding ORgansation Details
        {
            var organisation = new Ncmorg
            {
                Oid = newOrganisation.oid,
                Oname = newOrganisation.Oname,
                Spoc = newOrganisation.Spoc,
                Styear = newOrganisation.styear,
                Contname = newOrganisation.Contname,
                Contemail = newOrganisation.Contemail,
                Oactive = 1 // Assuming all new organizations are active by default
            };

            _EcheckContext.Ncmorgs.Add(organisation);
            await _EcheckContext.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> CheckDuplicateAsync(OrganisationGeneralInfoViewModel newOrganisation)
        //{
        //    // Check if an organization with the same OID or Organisation Name exists
        //    return await _EcheckContext.Ncmorgs.AnyAsync(o =>
        //        o.Oid == newOrganisation.oid || o.Oname == newOrganisation.Oname);
        //}

        public async Task<CombinedOrganisationSetupViewModel> GetOrganisationSetupAsync(string searchTerm, string? selectedOid)                // service for getting organisation list and general info of that organisation
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

        public async Task<bool> UpdateOrganisationInfoAsync(OrganisationGeneralInfoViewModel updatedInfo)          // update the details in general info 
        {
            var organisation = await _EcheckContext.Ncmorgs
                .FirstOrDefaultAsync(o => o.Oid == updatedInfo.oid);

            if (organisation == null)
            {
                return false;
            }

            // Update the fields, allowing null values
            organisation.Oname = updatedInfo.Oname;
            organisation.Spoc = updatedInfo.Spoc;
            organisation.Styear = updatedInfo.styear; 
            organisation.Contname = updatedInfo.Contname;
            organisation.Contemail = updatedInfo.Contemail;    
           
            // Save changes to the database  
            await _EcheckContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<AddLocationViewModel>> GetLocationDatabyOidAsync(string oid)                // getting location data on basis of oid for LOCATIONSDATA button
        {
            return await _EcheckContext.Ncmlocs
                .Where(n => n.Oid == oid)
                .Select(n => new AddLocationViewModel
                {
                   // Oid = oid,
                    Lcode = n.Lcode,
                    Lname = n.Lname,
                    Lcity = n.Lcity,
                    Lstate = n.Lstate,
                    Lregion = n.Lregion,
                    Iscentral = n.Iscentral,    
                    Iscloc = n.Iscloc,
                    Lactive = n.Lactive,
                    Ltype = n.Ltype,
                    Lsetup = n.Lsetup
                }).ToListAsync();
        }

        //public async Task<bool> AddLocationDataAsync (List<AddLocationViewModel> addlocationdata)                      // adding additional information for ADDLOCATIONS button
        //{
        //    foreach (var loc in addlocationdata)
        //    {
        //        var locationInDb = await _EcheckContext.Ncmlocs.FirstOrDefaultAsync(n=>n.Lcode == loc.Lcode && n.Oid == loc.Oid);
        //        if (locationInDb != null)
        //        {
        //            locationInDb.Iscentral = loc.Iscentral;
        //            locationInDb.Iscloc = loc.Iscloc;
        //            locationInDb.Lactive = loc.Lactive;
        //        }
        //    }

        //    await _EcheckContext.SaveChangesAsync();
        //    return true;
        //}
        public async Task<bool> AddLocationDataAsync(List<AddLocationViewModel> addlocationdata)
        {
            foreach (var loc in addlocationdata)
            {
                var locationInDb = await _EcheckContext.Ncmlocs.FirstOrDefaultAsync(n => n.Lcode == loc.Lcode && n.Oid == loc.Oid);
                if (locationInDb != null)
                {
                    // Update all fields
                    locationInDb.Lname = loc.Lname;
                    locationInDb.Lcity = loc.Lcity;
                    locationInDb.Lstate = loc.Lstate;
                    locationInDb.Lregion = loc.Lregion;
                    locationInDb.Iscentral = loc.Iscentral;
                    locationInDb.Iscloc = loc.Iscloc;
                    locationInDb.Lactive = loc.Lactive;
                    locationInDb.Ltype = loc.Ltype;
                    locationInDb.Lsetup = loc.Lsetup;
                }
            }

            await _EcheckContext.SaveChangesAsync();
            return true;
        }
    }
}
