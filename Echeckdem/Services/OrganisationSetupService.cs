using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Globalization;
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
                generatedOid = Guid.NewGuid().ToString("N").Substring(0, 10);

                Console.WriteLine($"Generated Oid: {generatedOid}");
            }
            while (await _EcheckContext.Ncmorgs.AnyAsync(org => org.Oid == generatedOid));

            var spocEmail = await _EcheckContext.Ncusers
            .Where(u => u.Oid == "C4HR" && u.Uname == newOrganisation.Spoc)
            .Select(u => u.Emailid)
            .FirstOrDefaultAsync();

            var organisation = new Ncmorg
            {
                Oid = generatedOid,
                Oname = newOrganisation.Oname,
                Spoc = newOrganisation.Spoc,
                Styear = newOrganisation.styear,
                Contname = newOrganisation.Contname,
                Contemail = newOrganisation.Contemail,
                SpocEml = spocEmail,
                FileName = null,
                Oactive = 1                                     // Assuming all new organizations are active by default
            };

            _EcheckContext.Ncmorgs.Add(organisation);
            await _EcheckContext.SaveChangesAsync();
                        
            CreateOrganisationFolder(generatedOid);


            if (newOrganisation.PdfFile != null && newOrganisation.PdfFile.Length > 0)
            {
                string copiesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", generatedOid, "Copies");

                string uploadedFileName = Path.GetFileName(newOrganisation.PdfFile.FileName);
                string fullPath = Path.Combine(copiesFolder, uploadedFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await newOrganisation.PdfFile.CopyToAsync(stream);
                }
                organisation.FileName = uploadedFileName;
                _EcheckContext.Ncmorgs.Update(organisation);
                await _EcheckContext.SaveChangesAsync();
            }
                return true;
        }

        private void CreateOrganisationFolder(string oid)
        {
            string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", oid);

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath); // Create Main Folder

                // Create Subfolders
                Directory.CreateDirectory(Path.Combine(rootPath, "Contr"));
                Directory.CreateDirectory(Path.Combine(rootPath, "Reg"));
                Directory.CreateDirectory(Path.Combine(rootPath, "Ret"));
                Directory.CreateDirectory(Path.Combine(rootPath, "Bocw"));
                Directory.CreateDirectory(Path.Combine(rootPath, "Acts"));
                Directory.CreateDirectory(Path.Combine(rootPath, "Copies"));                            
                
            }
        }

        public async Task<List<SelectListItem>> GetC4HRSPOCListAsync()
        {
            return await _EcheckContext.Ncusers
                .Where(u => u.Oid == "C4HR")
                .Select(u => new SelectListItem
                {
                    Value = u.Uname,
                    Text = u.Uname
                })
                .ToListAsync();
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
                        spoc_eml = o.SpocEml,
                        styear = o.Styear,
                        Contname = o.Contname,
                        Contemail = o.Contemail,
                        Oactive = o.Oactive,
                        FileName = o.FileName
                    })
                    .FirstOrDefaultAsync();
            }

            return new CombinedOrganisationSetupViewModel
            {
                OrganisationsList = organisationsList,
                SelectedOrganisation = selectedOrganisation                                
            };
        }
        public async Task<bool> UpdateOrganisationInfoAsync(OrganisationGeneralInfoViewModel updatedInfo)                           // update the details in general info 
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


            if (updatedInfo.PdfFile != null && updatedInfo.PdfFile.Length > 0)
            {
                string copiesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", updatedInfo.oid, "Copies");

                if (!Directory.Exists(copiesFolder))
                {
                    Directory.CreateDirectory(copiesFolder);
                }

                string uploadedFileName = Path.GetFileName(updatedInfo.PdfFile.FileName);
                string fullPath = Path.Combine(copiesFolder, uploadedFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await updatedInfo.PdfFile.CopyToAsync(stream);
                }

                organisation.FileName = uploadedFileName;
            }

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
                }
                // Update all fields
                locationInDb.Lname = addlocationdata.Lname ?? locationInDb.Lname;
                locationInDb.Lcity = addlocationdata.Lcity ?? locationInDb.Lcity;
                locationInDb.Lstate = addlocationdata.Lstate ?? locationInDb.Lstate;
                locationInDb.Lregion = addlocationdata.Lregion ?? locationInDb.Lregion;
                locationInDb.Iscentral = addlocationdata.Iscentral ?? locationInDb.Iscentral;
                locationInDb.Iscloc = addlocationdata.Iscloc ?? locationInDb.Iscloc;
                locationInDb.Lactive = addlocationdata.Lactive ?? locationInDb.Lactive;
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
                    //  Resolve lcode based on lname
                           var site = boSites.FirstOrDefault(b => b.Lname.Equals(lname, StringComparison.OrdinalIgnoreCase));
                            if (site == null)
                                throw new InvalidOperationException($"Invalid Location Name (Lname): {lname} for BO site.");

                            var lcode = site.Lcode; // Use the resolved lcode for further processing

                            var resolvedLname = site.Lname; // Use the resolved lname for population

                            var projectAreaCell = row.Cell(7);
                            decimal? projectArea = null; // Use nullable decimal
                            if (!projectAreaCell.IsEmpty())
                            {
                                projectArea = projectAreaCell.GetValue<decimal>();
                            }

                            // Similarly for ProjectCostEst
                            var projectCostEstCell = row.Cell(8);
                            float? projectCostEst = null; // Use nullable float
                            if (!projectCostEstCell.IsEmpty())
                            {
                                projectCostEst = projectCostEstCell.GetValue<float?>();
                            }

                            var VendorCountCell = row.Cell(11);
                            int? VendorCount = null;
                            if(!VendorCountCell.IsEmpty())
                            {
                                VendorCount = VendorCountCell.GetValue<int?>();
                            }

                            var WorkerHeadCounCellt = row.Cell(12);
                            int? WorkerHeadCount = null;
                            if(!VendorCountCell.IsEmpty())
                            {
                                WorkerHeadCount = VendorCountCell.GetValue<int?>();
                            }

                            var projectStartDateCell = row.Cell(9);
                            DateOnly? projectStartDate = null;
                            if (!projectStartDateCell.IsEmpty())
                            {
                                if (projectStartDateCell.DataType == XLDataType.DateTime)
                                {
                                    // Read the date directly, Excel always stores dates in proper format
                                    projectStartDate = DateOnly.FromDateTime(projectStartDateCell.GetDateTime());
                                }
                                else
                                {
                                    // Handle cases where the date is stored as a text string
                                    string dateString = projectStartDateCell.GetString().Trim();
                                    if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedStartDate))
                                    {
                                        projectStartDate = DateOnly.FromDateTime(parsedStartDate);
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException($"Invalid date format in ProjectStartDateEst at row {row.RowNumber()}. Expected format: DD/MM/YYYY.");
                                    }
                                }
                            }

                            var projectEndDateCell = row.Cell(10);
                            DateOnly? projectEndDate = null;
                            if (!projectEndDateCell.IsEmpty())
                            {
                                if (projectEndDateCell.DataType == XLDataType.DateTime)   
                                {
                                    projectEndDate = DateOnly.FromDateTime(projectEndDateCell.GetDateTime());
                                }
                                else
                                {
                                    string dateString = projectEndDateCell.GetString().Trim();
                                    if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedEndDate))
                                    {
                                        projectEndDate = DateOnly.FromDateTime(parsedEndDate);
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException($"Invalid date format in ProjectEndDateEst at row {row.RowNumber()}. Expected format: DD/MM/YYYY.");
                                    }
                                }
                            }





                            var boDetail = new Ncmlocbo
                            {
                                Lcode = lcode,
                                Lname = resolvedLname,
                                ProjectCode = Guid.NewGuid().ToString().Substring(0, 6),
                                OvalId = row.Cell(2).GetValue<string>().Trim(),
                                ClientName = row.Cell(3).GetValue<string>().Trim(),
                                GeneralContractor = row.Cell(4).GetValue<string>().Trim(),
                                ProjectAddress = row.Cell(5).GetValue<string>().Trim(),
                                NatureofWork = row.Cell(6).GetValue<string>().Trim(),
                                //ProjectArea = row.Cell(7).GetValue<decimal>(),
                                //ProjectCostEst = row.Cell(8).GetValue<float?>(),
                                ProjectArea = projectArea, // Use the nullable value
                                ProjectCostEst = projectCostEst,
                                ProjectStartDateEst = projectStartDate,
                                ProjectEndDateEst = projectEndDate,
                                //VendorCount = row.Cell(11).GetValue<int>(),
                                VendorCount = VendorCount,
                                WorkerHeadCount = WorkerHeadCount,
                                //WorkerHeadCount = row.Cell(12).GetValue<int>(),
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

        public async Task<string> GetOidByLcodeAsync(string lcode)                                                                                        // fetch oid from nmcloc by taking lcode from ncmlocbo and matchingthat lcode in ncmlloc 
        {
            var query = "SELECT TOP 1 Oid FROM Ncmloc WHERE Lcode = @lcode";
            var oid = await _EcheckContext.Ncmlocs
                .FromSqlRaw(query, new SqlParameter("@lcode", lcode))
                .Select(x => x.Oid)
                .FirstOrDefaultAsync();
            return oid;
        }

        public async Task<List<string>> GetActiveScopesByLcodeAsync(string lcode)
        {
            var activeScopeNames = await( from map in _EcheckContext.BoScopeMaps 
                                          join scope in _EcheckContext.BocwScopes
                                          on map.ScopeId equals scope.ScopeId 
                                          where map.Lcode == lcode && map.Active == true
                                          select scope.ScopeName).ToListAsync();

            return activeScopeNames;
        }

        public async Task<List<Ncmlocbo>> GetAllBocwDetailsWithScopesAsync(string oid)
        {
            

            var query = @" 
                        SELECT* FROM Ncmlocbo
                       WHERE Lcode IN(
                       SELECT Lcode
                       FROM Ncmloc
                       WHERE Oid = @oid AND Ltype = 'bo'
                       )";


          

            var bocwDetails = await _EcheckContext.Ncmlocbos
                .FromSqlRaw(query, new SqlParameter("@oid", oid))
                .ToListAsync();
       

            foreach(var bo in bocwDetails)
            {
                var activeScopes = await GetActiveScopesByLcodeAsync(bo.Lcode);
                bo.ActiveScopes = string.Join(",", activeScopes);   
            }

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
            return await _EcheckContext.BocwScopes.Where(scope => scope.ScopeActive == 1).ToListAsync();
        }


        public async Task AddOrUpdateMapping(string lcode, string projectCode, List<string> selectedScopeIds)
        {
            // Fetch all existing active mappings for this site & project
            var existingMappings = await _EcheckContext.BoScopeMaps.Where(m => m.Lcode == lcode && m.ProjectCode == projectCode && m.Active).ToListAsync();

            var existingScopeIds = existingMappings.Select(m => m.ScopeId).ToHashSet(); // Fast lookup

            // Step 1: Update existing mappings - Set Active = true for selected ones, false for others
            foreach (var mapping in existingMappings)
            {

                //mapping.Active = selectedScopeIds.Contains(mapping.ScopeId);

                if (selectedScopeIds.Contains(mapping.ScopeId))
                {
                    mapping.Active = true; 
                }
                else
                {
                    mapping.Active = false;
                }
                    
            }

            // Step 2: Identify new scopes to add (those that don't already exist in active mappings)
            var scopesToAdd = selectedScopeIds.Except(existingScopeIds).ToList();

            // Step 3: Add only new scopes that are not already in the DB
            foreach (var scopeId in scopesToAdd)
            {
                // Check if the new scope already exists for this Lcode and ProjectCode and is not marked as active
                var existingScope = await _EcheckContext.BoScopeMaps.FirstOrDefaultAsync(m => m.ScopeId == scopeId && m.Lcode == lcode && m.ProjectCode == projectCode);

                if (existingScope == null) // If the scope doesn't exist, insert a new mapping
                {
                    var newMapping = new BoScopeMap
                    {
                        ScopeMapId = Guid.NewGuid().ToString("N").Substring(0, 6), // Unique ID
                        ScopeId = scopeId,
                        Lcode = lcode,
                        ProjectCode = projectCode,
                        Active = true  // New scopes should be active
                    };

                    _EcheckContext.BoScopeMaps.Add(newMapping);
                }
                else
                {
                // If the scope exists but is not active, update its status to Active
                    if (!existingScope.Active)
                    {
                        existingScope.Active = true; // Ensure it is set to active
                    }
                }
            }

            // Save changes once to optimize database writes 
            await _EcheckContext.SaveChangesAsync();
        }

        
        // --------------------------------------PROJECT SETUP AFTER SCOPE SETUP --------------------------------------------//


        public async Task<int> ProjectSetupAsync(string lcode, string projectCode, string scopeId, int status)
        {
            // fetching lname from ncmlocbo
            var lname = await _EcheckContext.Ncmlocbos.Where(n=>n.Lcode==lcode).Select(n=>n.Lname).FirstOrDefaultAsync();

            if (lname == null)
                throw new InvalidOperationException("Site not found for the specified Lcode.");

            // Fetch the scopeId from BoScopeMap based on lcode and projectCode
            var fetchedScopeId = await _EcheckContext.BoScopeMaps.Where(bsm => bsm.Lcode == lcode && bsm.ProjectCode == projectCode && bsm.Active).Select(bsm => bsm.ScopeId).FirstOrDefaultAsync();

            if (fetchedScopeId == null) 
                throw new InvalidOperationException("ScopeId not found for the specified Lcode and ProjectCode.");

            // Fetch WorkId and DueDate from TrackScope
            var trackScope = await _EcheckContext.TrackScopes.Where(ts=>ts.ScopeId== fetchedScopeId).FirstOrDefaultAsync();

            if (trackScope == null)
                throw new InvalidOperationException("TrackScope not found for the specified ScopeId.");

            // Fetch ProjectStartDateEst from Ncmlocbo
            var projectStartDateEst = await _EcheckContext.Ncmlocbos.Where(n => n.Lcode == lcode && n.ProjectCode == projectCode).Select(n => n.ProjectStartDateEst).FirstOrDefaultAsync();

            if (projectStartDateEst == null)
                throw new InvalidOperationException("ProjectStartDateEst not found for the specified Lcode and ProjectCode.");

            // Calcualte DUeDate for ncbocw
            
            DateTime startDateTime = new DateTime(projectStartDateEst.Value.Year, projectStartDateEst.Value.Month, projectStartDateEst.Value.Day);
            int daysToAdd = trackScope.DueDate ?? 0; // Get the number of days to add
            DateTime calculatedDueDate = startDateTime.AddDays(daysToAdd); // Add days
            DateOnly dueDate = DateOnly.FromDateTime(calculatedDueDate); // Convert back to DateOnly

            var CreateDate = DateOnly.FromDateTime(DateTime.Now);

            int calculatedStatus = CreateDate > dueDate ? 0 : 2;
            var ncbocw = new Ncbocw
            {
                Lcode = lcode,
                ProjectCode = projectCode,
                Lname = lname,
                ScopeId = fetchedScopeId,
                ScopeMapId = await _EcheckContext.BoScopeMaps.Where(bsm => bsm.ScopeId == fetchedScopeId && bsm.Lcode == lcode && bsm.ProjectCode == projectCode && bsm.Active).Select(bsm => bsm.ScopeMapId).FirstOrDefaultAsync(),
                WorkId = trackScope.WorkId,
                DueDate = dueDate,
                FirstAlert = DateOnly.FromDateTime(DateTime.Now),
                Status = calculatedStatus,
                Task = trackScope.Task,
                CreateDate = CreateDate
            };
            _EcheckContext.Ncbocws.Add(ncbocw);
            return await _EcheckContext.SaveChangesAsync();
        }



    }
}
