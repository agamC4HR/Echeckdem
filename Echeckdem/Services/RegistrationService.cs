using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZstdSharp.Unsafe;
using Echeckdem.CustomFolder.Dashboard.Registration;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Vml.Office;
using Echeckdem.ViewModel.Shared;
using Echeckdem.ViewModel.ComplianceTracker;
namespace Echeckdem.Services
{
    public class RegistrationService
    {
        private readonly DbEcheckContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpContext _httpContext;
        public RegistrationService(DbEcheckContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<List<ComplianceViewModel>> GetRegistrationAsync(FilterFormModel model=null ) 
        {
            if (_httpContext.Session.GetInt32("User Level") == 1)
            {


                if (model == null)
                {

                    return await (from reg in _context.Ncregs
                                  join loc in _context.Ncmlocs on reg.Lcode equals loc.Lcode
                                  join org in _context.Ncmorgs on loc.Oid equals org.Oid

                                  where loc.Lactive == 1 && org.Oactive == 1
                                  orderby reg.Doe ascending
                                  select new ComplianceViewModel
                                  {
                                      Id = reg.Uid,
                                      Oid = org.Oid,
                                      Lcode = loc.Lcode,
                                      Oname = org.Oname,
                                      Lname = loc.Lname,
                                      Lcity = loc.Lcity,
                                      Lstate = loc.Lstate,
                                      ServiceType = "Registration",
                                      Service = (from mastre in _context.Mastregs where mastre.Rtype == reg.Tp select mastre.Rdesc).FirstOrDefault() ?? "Unknown",
                                      Period = "N/A",
                                      DueDate = reg.Doe.HasValue ? reg.Doe.Value.ToString("dd-MMM-yyyy") : string.Empty,
                                      CompletionDate = reg.Dolr.HasValue ? reg.Dolr.Value.ToString("dd-MMM-yyyy") : string.Empty,
                                      Status = reg.Status,
                                      FileName = reg.Filename

                                  }).ToListAsync();


                }
                else
                {
                    var registrations = await (from reg in _context.Ncregs
                                               join loc in _context.Ncmlocs on reg.Lcode equals loc.Lcode
                                               join org in _context.Ncmorgs on loc.Oid equals org.Oid

                                               where loc.Lactive == 1 && org.Oactive == 1
                                                 && (string.IsNullOrEmpty(model.SelectedClient) || org.Oid == model.SelectedClient)
                                                 && (string.IsNullOrEmpty(model.SelectedSite) || loc.Lcode == model.SelectedSite)
                                                 && (string.IsNullOrEmpty(model.SelectedState) || loc.Lstate == model.SelectedState)
                                                 && (string.IsNullOrEmpty(model.SelectedCity) || loc.Lcity == model.SelectedCity)
                                                 && (!model.StartDueDate.HasValue || reg.Doe >= model.StartDueDate.Value)
                                                 && (!model.EndDueDate.HasValue || reg.Doe <= model.EndDueDate.Value)
                                               orderby reg.Doe ascending
                                               select new ComplianceViewModel
                                               {
                                                   Id = reg.Uid,
                                                   Oid = org.Oid,
                                                   Lcode = loc.Lcode,
                                                   Oname = org.Oname,
                                                   Lname = loc.Lname,
                                                   Lcity = loc.Lcity,
                                                   Lstate = loc.Lstate,
                                                   ServiceType = "Registration",
                                                   Service = (from mastre in _context.Mastregs where mastre.Rtype == reg.Tp select mastre.Rdesc).FirstOrDefault() ?? "Unknown",
                                                   Period = "N/A",
                                                   DueDate = reg.Doe.HasValue ? reg.Doe.Value.ToString("dd-MMM-yyyy") : string.Empty,
                                                   CompletionDate = reg.Dolr.HasValue ? reg.Dolr.Value.ToString("dd-MMM-yyyy") : string.Empty,
                                                   Status = reg.Status,
                                                   FileName = reg.Filename

                                               }).ToListAsync();

                    return registrations;
                }
            }
            else
            {
                var uno = _httpContext.Session.GetInt32("UNO");

                if (model == null)
                {

                    return await (from reg in _context.Ncregs
                                  join loc in _context.Ncmlocs on reg.Lcode equals loc.Lcode
                                  join org in _context.Ncmorgs on loc.Oid equals org.Oid
                                  join usem in _context.Ncumaps on new { loc.Oid, loc.Lcode } equals new { usem.Oid, usem.Lcode }
                                  where usem.Uno == uno && loc.Lactive == 1 && org.Oactive == 1
                                  orderby reg.Doe ascending
                                  select new ComplianceViewModel
                                  {
                                      Id = reg.Uid,
                                      Oid = org.Oid,
                                      Lcode = loc.Lcode,
                                      Oname = org.Oname,
                                      Lname = loc.Lname,
                                      Lcity = loc.Lcity,
                                      Lstate = loc.Lstate,
                                      ServiceType = "Registration",
                                      Service = (from mastre in _context.Mastregs where mastre.Rtype == reg.Tp select mastre.Rdesc).FirstOrDefault() ?? "Unknown",
                                      Period = "N/A",
                                      DueDate = reg.Doe.HasValue ? reg.Doe.Value.ToString("dd-MMM-yyyy") : string.Empty,
                                      CompletionDate = reg.Dolr.HasValue ? reg.Dolr.Value.ToString("dd-MMM-yyyy") : string.Empty,
                                      Status = reg.Status,
                                      FileName = reg.Filename

                                  }).ToListAsync();


                }
                else
                {
                    var registrations = await (from reg in _context.Ncregs
                                               join loc in _context.Ncmlocs on reg.Lcode equals loc.Lcode
                                               join org in _context.Ncmorgs on loc.Oid equals org.Oid
                                               join usem in _context.Ncumaps on new { loc.Oid, loc.Lcode } equals new { usem.Oid, usem.Lcode }
                                               where usem.Uno == uno && loc.Lactive == 1 && org.Oactive == 1
                                                 && (string.IsNullOrEmpty(model.SelectedClient) || org.Oid == model.SelectedClient)
                                                 && (string.IsNullOrEmpty(model.SelectedSite) || loc.Lcode == model.SelectedSite)
                                                 && (string.IsNullOrEmpty(model.SelectedState) || loc.Lstate == model.SelectedState)
                                                 && (string.IsNullOrEmpty(model.SelectedCity) || loc.Lcity == model.SelectedCity)
                                                 && (!model.StartDueDate.HasValue || reg.Doe >= model.StartDueDate.Value)
                                                 && (!model.EndDueDate.HasValue || reg.Doe <= model.EndDueDate.Value)
                                               orderby reg.Doe ascending
                                               select new ComplianceViewModel
                                               {
                                                   Id = reg.Uid,
                                                   Oid = org.Oid,
                                                   Lcode = loc.Lcode,
                                                   Oname = org.Oname,
                                                   Lname = loc.Lname,
                                                   Lcity = loc.Lcity,
                                                   Lstate = loc.Lstate,
                                                   ServiceType = "Registration",
                                                   Service = (from mastre in _context.Mastregs where mastre.Rtype == reg.Tp select mastre.Rdesc).FirstOrDefault() ?? "Unknown",
                                                   Period = "N/A",
                                                   DueDate = reg.Doe.HasValue ? reg.Doe.Value.ToString("dd-MMM-yyyy") : string.Empty,
                                                   CompletionDate = reg.Dolr.HasValue ? reg.Dolr.Value.ToString("dd-MMM-yyyy") : string.Empty,
                                                   Status = reg.Status,
                                                   FileName = reg.Filename

                                               }).ToListAsync();

                    return registrations;
                }

            }

                
        }
 
        
        public Task<List<CompliantRegistrationViewModel>> GetCompliantRegistrationsAsync(int uno, int selectedYear)
        {
            // LINQ query to get the compliant registrations
            var registrations = (from reg in _context.Ncregs
                                 join loc in _context.Ncmlocs on reg.Lcode equals loc.Lcode
                                 where reg.Status == "C"
                                       && reg.Doe.HasValue
                                       && reg.Doe.Value.Year == selectedYear
                                       && _context.Ncumaps.Any(m => m.Uno == uno && m.Oid == reg.Oid && m.Lcode == reg.Lcode)
                                 select new
                                 {
                                     reg.Lcode,
                                     reg.Doe,
                                     loc.Lname,
                                     loc.Ltype
                                 })
                                 .AsEnumerable() // Force client-side evaluation
                                 .Where(x => (x.Doe.Value.ToDateTime(new TimeOnly(0, 0)) - DateTime.Now).Days > 60)
                                 .Select(x => new CompliantRegistrationViewModel
                                 {
                                     Lcode = x.Lcode,
                                     Lname = x.Lname,
                                     Ltype = x.Ltype,
                                     Doe = x.Doe
                                 })
                                 .ToList(); // No need for await as this is now synchronous

            return Task.FromResult(registrations); // Return the result as a Task
        }


        public async Task<List<string>> GetOrganizationNamesAsync(int uno)     // code for getting oname on basis of uno and oid in filters)

        {
            var sqlQuery = @" 
            SELECT DISTINCT c.OName
            FROM NCUMAP m
            JOIN NCMORG c ON m.Oid = c.Oid
            WHERE m.Uno = {0} AND c.OActive = 1 "; 

            var organizationNames = await _context.Ncmorgs
           .FromSqlRaw(sqlQuery, uno)
           .Select(o => o.Oname ?? string.Empty)
           .Where(o => !string.IsNullOrEmpty(o))
           .ToListAsync();

            return organizationNames;
        }
        public async Task<List<string>> GetLocationNamesAsync(int uno)         // Get all locations name for filters
        {
            var sqlQuery = @"
            SELECT DISTINCT b.Lname
            FROM NCUMAP m
            JOIN NCMLOC b on m.Lcode = b.Lcode 
            WHERE m.Uno = {0} AND b.Lactive = 1";

            var LocationNames = await _context.Ncmlocs
            .FromSqlRaw(sqlQuery, uno)
            .Select(l => l.Lname ?? string.Empty)
            .Where(l => !string.IsNullOrEmpty(l))
            .ToListAsync();

            return LocationNames;
        }

        public async Task<List<string>> GetFilteredLocationNamesAsync(int uno, string organizationName)          //Getting all filtered location on the basis of organisation name selected.
        {
            var sqlQuery = @"
            SELECT DISTINCT b.Lname
            FROM NCUMAP m
            JOIN NCMLOC b ON m.Lcode = b.Lcode
            JOIN NCMORG c ON m.Oid = c.Oid
            WHERE m.Uno = @uno 
            AND b.Lactive = 1
            AND c.OActive = 1
            AND c.OName = @organizationName";

            var locationNames = await _context.Ncmlocs
            .FromSqlRaw(sqlQuery,
            new SqlParameter("@uno", uno),
            new SqlParameter("@organizationName", organizationName))
            .Select(l => l.Lname ?? string.Empty)
            .Where(l => !string.IsNullOrEmpty(l))
            .ToListAsync();

            return locationNames;
        }

        public async Task<List<string>> GetStateNamesAsync(int uno)
        {
            var sqlQuery = @"
            SELECT DISTINCT c.Statedesc
            FROM NCUMAP m
            JOIN NCMLOC b ON m.Lcode = b.Lcode
            JOIN MASTSTATES c ON b.lstate = c.stateid
            WHERE m.Uno = {0} AND b.Lactive = 1";

            var stateNames = await _context.Database
           .SqlQueryRaw<string>(sqlQuery, uno)
           .ToListAsync();

            return stateNames;
        }

        public async Task<List<string>> GetCityNamesAsync(int uno)  
        {
            var sqlQuery = @"
            SELECT DISTINCT b.Lcity
            FROM NCUMAP m
            JOIN NCMLOC b ON m.Lcode = b.Lcode
            WHERE m.Uno = {0} AND b.Lactive = 1";

            var cityNames = await _context.Ncmlocs
            .FromSqlRaw(sqlQuery, uno)
            .Select(c => c.Lcity ?? string.Empty)
            .Where(c => !string.IsNullOrEmpty(c))
            .ToListAsync();

            return cityNames;
        }
        public async Task<Ncreg> GetByIdAsync(int uid, string oid, string lcode)
        {
            var registration = await _context.Ncregs
         .FirstOrDefaultAsync(r => r.Uid == uid && r.Oid == oid && r.Lcode == lcode);

            if (registration == null)
            {
                Console.WriteLine("⚠️ No record found in database!");
            }

            return registration;
        }


        public async Task<string> UpdateRegAsync (int? uid, string oid, string lcode, string status, string rno, int noe, string nmoe, DateOnly? doi, DateOnly? doe, DateOnly? dolr, string remarks, IFormFile file )
        {
            try
            {
                var ncregRecord = uid.HasValue
                    ? await _context.Ncregs.FirstOrDefaultAsync(n => n.Uid == uid && n.Oid == oid && n.Lcode == lcode)
                    : new Ncreg();

                if (ncregRecord == null && uid.HasValue)
                {
                    return "Record not found.";
                }

                ncregRecord.Oid = oid;
                ncregRecord.Lcode = lcode;
                ncregRecord.Status = status;
                ncregRecord.Rno = rno;
                ncregRecord.Noe = noe;
                ncregRecord.Nmoe = nmoe;
                ncregRecord.Doi = doi;
                ncregRecord.Doe = doe;
                ncregRecord.Dolr = dolr;
                ncregRecord.Remarks = remarks;

                if(file != null && file.Length>0)
                {
                    if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                    {
                        return "Only PDF files are allowed.";
                    }

                    string originalFileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(originalFileName);
                    string nameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                    if (originalFileName.Length > 50)
                    {
                        return "Filename too long. Must be 50 characters or fewer.";
                    }


                    string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid.ToString(), "REG"); 
                    Directory.CreateDirectory(folderPath);

                    string filePath = Path.Combine(folderPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    ncregRecord.Filename = file.FileName;
                }

                if (!uid.HasValue)
                {
                    _context.Ncregs.Add(ncregRecord);
                }
                else
                {
                    _context.Ncregs.Update(ncregRecord);
                }

                await _context.SaveChangesAsync();
                return "Data Saved Successfully!!!";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
