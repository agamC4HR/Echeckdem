using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Echeckdem.Services
{
    public class OrganisationSetupService
    {
        private readonly DbEcheckContext _EcheckContext;

        public OrganisationSetupService(DbEcheckContext EcheckContext)
        {
            _EcheckContext = EcheckContext;
        }

        public async Task<bool> AddOrganisationAsync(OrganisationGeneralInfoViewModel newOrganisation)                        // Adding ORgansation Details (setting up new organisation)
        {

            string generatedOid;
            do
            {
                //Guid abc = Guid.NewGuid();
                generatedOid = Guid.NewGuid().ToString("N").Substring(0, 10);

                Console.WriteLine($"Generated Oid: {generatedOid}");
            }
            while (await _EcheckContext.Ncmorgs.AnyAsync(org => org.Oid == generatedOid));

            var organisation = new Ncmorg
            {
                // Oid = newOrganisation.oid,
                Oid = generatedOid,
                Oname = newOrganisation.Oname,
                Spoc = newOrganisation.Spoc,
                Styear = newOrganisation.styear,
                Contname = newOrganisation.Contname,
                Contemail = newOrganisation.Contemail,
                Oactive = 1                                     // Assuming all new organizations are active by default
            };

            _EcheckContext.Ncmorgs.Add(organisation);
            await _EcheckContext.SaveChangesAsync();
            return true;
        }

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
                        Contemail = o.Contemail,
                        Oactive = o.Oactive,
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
            organisation.Oactive = updatedInfo.Oactive;

            // Save changes to the database  
            await _EcheckContext.SaveChangesAsync();
            return true;
        }

        public Task<List<AddLocationViewModel>> GetLocationDatabyOidAsync(string oid)                // getting location data on basis of oid for LOCATIONSDATA button

        {
            Console.WriteLine($"Fetching locations for OID: {oid}");

            return _EcheckContext.Ncmlocs
                .Where(n => n.Oid == oid)
                .Select(n => new AddLocationViewModel
                {
                    Oid = oid,
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
        public async Task<bool> AddLocationDataAsync(CombinedOrganisationSetupViewModel addlocationdata)              // Editing the details for LOCatiion DATA button
        {

            var locationInDb = await _EcheckContext.Ncmlocs.FirstOrDefaultAsync(n => n.Lcode == addlocationdata.Lcode && n.Oid == addlocationdata.Oid);

            if (locationInDb != null)
            {
                if (locationInDb.Ltype?.Contains("BO") == true)
                {
                    var isNcmLocBoPopulated = await _EcheckContext.Ncmlocbos.AnyAsync(n => n.Lcode == locationInDb.Lcode);

                    //if (!isNcmLocBoPopulated)  
                    //{
                    //    throw new InvalidOperationException($"Additional information is required for BOCW site (Lcode: {locationInDb.Lcode} and Lname: {locationInDb.Lname}).");
                    //}
                }
                // Update all fields

                locationInDb.Lname = addlocationdata.Lname ?? locationInDb.Lname;
                locationInDb.Lcity = addlocationdata.Lcity ?? locationInDb.Lcity;
                locationInDb.Lstate = addlocationdata.Lstate ?? locationInDb.Lstate;
                locationInDb.Lregion = addlocationdata.Lregion ?? locationInDb.Lregion;
                locationInDb.Iscentral = addlocationdata.Iscentral ?? locationInDb.Iscentral;
                locationInDb.Iscloc = addlocationdata.Iscloc ?? locationInDb.Iscloc;
                locationInDb.Lactive = addlocationdata.Lactive ?? locationInDb.Lactive;
                // locationInDb.Ltype = string.IsNullOrEmpty(addlocationdata.Ltype) ? null : addlocationdata.Ltype;
                locationInDb.Ltype = string.IsNullOrEmpty(addlocationdata.Ltype) ? locationInDb.Ltype : addlocationdata.Ltype;

                locationInDb.Lsetup = addlocationdata.Lsetup;

                await _EcheckContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> UploadBOCWSiteDetailsAsync(IFormFile file, string oid)                                              // POPULATING ncmlocbo
        {
            var boSites = await _EcheckContext.Ncmlocs.Where(n => n.Oid == oid && n.Ltype == "BO").Select(n => new { n.Lcode, n.Lname }).ToListAsync();

            if (!boSites.Any())
                throw new InvalidOperationException("No BO sites found for the specified OID.");

            var boDetails = new List<Ncmlocbo>();
            if (file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var workbook = new XLWorkbook(stream))

                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RangeUsed().RowsUsed().Skip(1);        // Skip header row

                        foreach (var row in rows)
                        {
                            var lname = row.Cell(1).GetValue<string>()?.Trim();

                            // Resolve lcode based on lname
                            var site = boSites.FirstOrDefault(b => b.Lname.Equals(lname, StringComparison.OrdinalIgnoreCase));
                            if (site == null)
                                throw new InvalidOperationException($"Invalid Location Name (Lname): {lname} for BO site.");

                            var lcode = site.Lcode; // Use the resolved lcode for further processing

                            //var lcode = row.Cell(1).GetValue<string>()?.Trim();
                            //if (!boSites.Any(b => b.Lcode == lcode))
                            //    throw new InvalidOperationException($"Invalid Lcode: {lcode} for BO site.");



                            //var matchingSite = boSites.FirstOrDefault(b => b.Lcode == lcode);
                            //if (matchingSite == null)
                            //    throw new InvalidOperationException($"Invalid Lcode: {lcode} for BO site.");

                            //var lname = matchingSite.Lname; // Get lname from Ncmloc table





                            // Logic to handle date fromat 
                            var projectStartDateValue = row.Cell(9).GetValue<string>()?.Trim();
                            DateOnly? projectStartDate = null;
                            if (!string.IsNullOrEmpty(projectStartDateValue))
                            {
                                if (DateTime.TryParse(projectStartDateValue, out var parsedStartDate))
                                {
                                    projectStartDate = DateOnly.FromDateTime(parsedStartDate.Date);  // Strips the time
                                }
                                else
                                {
                                    throw new InvalidOperationException($"Invalid date format in ProjectStartDateEst: {projectStartDateValue}");
                                }
                            }

                            // Logic to handle date fromat 
                            var projectEndDateValue = row.Cell(10).GetValue<string>()?.Trim();
                            DateOnly? projectEndDate = null;
                            if (!string.IsNullOrEmpty(projectEndDateValue))
                            {
                                if (DateTime.TryParse(projectEndDateValue, out var parsedEndDate))
                                {
                                    projectEndDate = DateOnly.FromDateTime(parsedEndDate.Date);
                                }
                                else
                                {
                                    throw new InvalidOperationException($"Invalid date format in ProjectEndDateEst: {projectEndDateValue}");
                                }
                            }

                            var boDetail = new Ncmlocbo
                            {
                                Lcode = lcode,
                                //Lname = lname,
                                ProjectCode = Guid.NewGuid().ToString().Substring(0, 6), // Generate 6-char project code
                                OvalId = row.Cell(2).GetValue<string>().Trim(),
                                ClientName = row.Cell(3).GetValue<string>().Trim(),
                                GeneralContractor = row.Cell(4).GetValue<string>().Trim(),
                                ProjectAddress = row.Cell(5).GetValue<string>().Trim(),
                                NatureofWork = row.Cell(6).GetValue<string>().Trim(),
                                ProjectArea = row.Cell(7).GetValue<decimal>(),
                                ProjectCostEst = row.Cell(8).GetValue<float?>(),
                                ProjectStartDateEst = projectStartDate,
                                ProjectEndDateEst = projectEndDate,
                                VendorCount = row.Cell(11).GetValue<int>(),
                                WorkerHeadCount = row.Cell(12).GetValue<int>(),
                                ProjectLead = row.Cell(13).GetValue<string>().Trim(),

                            };
                            boDetails.Add(boDetail);
                        }
                    }
                    await _EcheckContext.Ncmlocbos.AddRangeAsync(boDetails);
                    await _EcheckContext.SaveChangesAsync();
                }

                return boDetails.Count;

            }
            return 0;
        }


        public async Task<List<Ncmloc>> GetBoSitesAsync(string oid)                                      // function to get sites under BOCW ACT.
        {
            return await _EcheckContext.Ncmlocs
                .Where(n => n.Oid == oid && n.Ltype == "BO").ToListAsync();
        }


        public async Task<bool> HasIncompleteBODataAsync(string oid)                                                  // Function to get ORG details in which ncmlocbo table is not populated yet.
        {
            var boSites = await GetBoSitesAsync(oid);
            if (!boSites.Any())
                return false;

            return boSites.Any(site => !_EcheckContext.Ncmlocbos.Any(b => b.Lcode == site.Lcode));
        }





        public async Task<List<Ncmlocbo>> GetAllBocwDetailsAsync(string oid)
        {
            var query = @"
                        SELECT * 
                        FROM Ncmlocbo 
                        WHERE Lcode IN (
                        SELECT Lcode 
                        FROM Ncmloc 
                        WHERE Oid = @oid
                        )";

            var bocwDetails = await _EcheckContext.Ncmlocbos
                .FromSqlRaw(query, new SqlParameter("@oid", oid))
                .ToListAsync();

            return bocwDetails;



        }



        public async Task<Ncmlocbo> GetBocwDetailsByLcodeAsync(string lcode)
        {
            return await _EcheckContext.Ncmlocbos.FirstOrDefaultAsync(b => b.Lcode == lcode);
        }


        public async Task UpdateBoDetailsAsync(Ncmlocbo updatedBoDetail)
        {
            var existingBoDetail = await _EcheckContext.Ncmlocbos.AsTracking().FirstOrDefaultAsync(b => b.Lcode == updatedBoDetail.Lcode);
            if (existingBoDetail != null)
            {
                existingBoDetail.OvalId = updatedBoDetail.OvalId;
                existingBoDetail.ClientName = updatedBoDetail.ClientName;
                existingBoDetail.GeneralContractor = updatedBoDetail.GeneralContractor;
                existingBoDetail.ProjectAddress = updatedBoDetail.ProjectAddress;
                existingBoDetail.NatureofWork = updatedBoDetail.NatureofWork;
                existingBoDetail.ProjectArea    = updatedBoDetail.ProjectArea;
                existingBoDetail.ProjectCostEst = updatedBoDetail.ProjectCostEst;
                existingBoDetail.ProjectStartDateEst = updatedBoDetail.ProjectStartDateEst;
                existingBoDetail.ProjectEndDateEst = updatedBoDetail.ProjectEndDateEst;
                existingBoDetail.VendorCount = updatedBoDetail.VendorCount;
                existingBoDetail.WorkerHeadCount = updatedBoDetail.WorkerHeadCount;
                existingBoDetail.ProjectLead = updatedBoDetail.ProjectLead;

                _EcheckContext.Entry(existingBoDetail).State = EntityState.Modified;

                await _EcheckContext.SaveChangesAsync();

            }

            else
            {
               throw new KeyNotFoundException("BO detail not found.");
            }
        }


        public async Task<List<Ncmlocbo>> GetAllSitesAsync()
        {
            return await _EcheckContext.Ncmlocbos.ToListAsync();
        }
        public async Task<List<BocwScope>> GetScopesAsync()
        {
            return await _EcheckContext.BocwScopes.ToListAsync();
        }

        public async Task AddOrUpdateMapping (string lcode, string projectCode, List<string> selectedScopeIds)
        {
            // Remove existing mappings for the site
            var existingMappings = _EcheckContext.BoScopeMaps.Where(m => m.Lcode == lcode && m.ProjectCode == projectCode);
            _EcheckContext.BoScopeMaps.RemoveRange(existingMappings);

            foreach (var scopeId in selectedScopeIds)
            {
                //// Ensure the ScopeId exists in the BocwScope table
                //var scope = await _EcheckContext.BocwScopes.FirstOrDefaultAsync(s => s.ScopeId == scopeId);

                //if (scope == null)
                //{
                //    // Handle error if no matching scope is found
                //    throw new InvalidOperationException($"Scope with ID '{scopeId}' does not exist.");
                //    //continue; // or throw an exception, depending on your requirements
                //}

                //var mapping = new BoScopeMap
                //{
                //    ScopeMapId = Guid.NewGuid().ToString("N").Substring(0, 6),
                //    ScopeId = scopeId,
                //    Lcode = lcode,
                //    ProjectCode = projectCode,
                //    Active = true
                //};

                //// Associating the existing scope with the mapping
                //mapping.LcodeNavigation = new Ncmloc { Lcode = lcode }; // Set LcodeNavigation if it's required
                //mapping.ProjectCodeNavigation = new Ncmlocbo { ProjectCode = projectCode }; // Set ProjectCodeNavigation
                ////mapping.Scope = scope; // This ensures the mapping is properly associated with the existing scope
                //mapping.ScopeMap = new BocwScope { BoScopeMapScopeMap = mapping };

                //_EcheckContext.BoScopeMaps.Add(mapping);
                var mapping = new BoScopeMap
                {
                    ScopeMapId = Guid.NewGuid().ToString("N").Substring(0, 6),
                    ScopeId = scopeId,
                    Lcode = lcode,
                    ProjectCode = projectCode,
                    Active = true

                };

                //mapping.LcodeNavigation = new Ncmloc { Lcode = lcode }; // Set LcodeNavigation if it's required
                //mapping.ProjectCodeNavigation = new Ncmlocbo { ProjectCode = projectCode }; // Set ProjectCodeNavigation
                //mapping.Scope = new BocwScope { ScopeId = scopeId }; // Set Scope (you can fetch full data from DB if needed)
               // mapping.ScopeMap = new BocwScope { BoScopeMapScopeMap = mapping };


                _EcheckContext.BoScopeMaps.Add(mapping);
            }
            await _EcheckContext.SaveChangesAsync();

        }
            
        

    }
}
