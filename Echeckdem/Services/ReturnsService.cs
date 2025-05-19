using Echeckdem.Models;
using Echeckdem.CustomFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mono.TextTemplating;
using System.Security.Policy;
using Microsoft.AspNetCore.Hosting;
using DocumentFormat.OpenXml.Spreadsheet;
using Echeckdem.ViewModel.Shared;
using Echeckdem.ViewModel.ComplianceTracker;

namespace Echeckdem.Services
{
    public class ReturnsService
    {
        private readonly DbEcheckContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpContext _httpContext;
        private static readonly Dictionary<int, string> StatusDescriptions = new()
{
            { -1,"Unknown"} ,
    { 0, "Future" },
    { 1, "Compliant" },
    { 2, "Non Compliant" },
    { 3, "Not IN Scope" },
    { 4, "Not Applicable" },
    { 5, "Under Process" }
    
};
        public ReturnsService(DbEcheckContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<List<ComplianceViewModel>> GetReturnAsync( FilterFormModel model = null)
        {
            if (_httpContext.Session.GetInt32("User Level") == 1)
            {
                if (model == null)
                {

                    var returns = await (from ret in _context.Ncrets
                                         join retemp in _context.Nctemprets on ret.Rcode equals retemp.Rcode
                                         join loc in _context.Ncmlocs on ret.Lcode equals loc.Lcode
                                         join org in _context.Ncmorgs on loc.Oid equals org.Oid
                                         where ret.Ryear == DateTime.Now.Year
                                         where loc.Lactive == 1 && org.Oactive == 1
                                         orderby ret.Lastdate ascending
                                         select new { ret, retemp, loc, org }).ToListAsync();
                    return returns.Select(x => new ComplianceViewModel
                    {
                        Id = x.ret.Rtid,
                        Oid = x.org.Oid,
                        Lcode = x.loc.Lcode,
                        Oname = x.org.Oname,
                        Lname = x.loc.Lname,
                        Lcity = x.loc.Lcity,
                        Lstate = x.loc.Lstate,
                        ServiceType = "Returns",
                        Service = x.retemp.Rtitle,
                        Period = x.ret.Ryear.ToString(),
                        DueDate = x.ret.Lastdate.HasValue ? x.ret.Lastdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        CompletionDate = x.ret.Depdate.HasValue ? x.ret.Depdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        Status = StatusDescriptions.TryGetValue(x.ret.Status ?? -1, out var description) ? description : "Unknown",
                        FileName = x.ret.Filename

                    }).ToList();
                }
                else
                {
                    var returns = await (from ret in _context.Ncrets
                                         join retemp in _context.Nctemprets on ret.Rcode equals retemp.Rcode
                                         join loc in _context.Ncmlocs on ret.Lcode equals loc.Lcode
                                         join org in _context.Ncmorgs on loc.Oid equals org.Oid
                                         
                                         where loc.Lactive == 1 && org.Oactive == 1
                                            && (string.IsNullOrEmpty(model.SelectedClient) || org.Oid == model.SelectedClient)
                                            && (string.IsNullOrEmpty(model.SelectedSite) || loc.Lcode == model.SelectedSite)
                                            && (string.IsNullOrEmpty(model.SelectedState) || loc.Lstate == model.SelectedState)
                                            && (string.IsNullOrEmpty(model.SelectedCity) || loc.Lcity == model.SelectedCity)
                                            && (model.StartDueDate == null || ret.Lastdate >= model.StartDueDate.Value)
                                            && (model.EndDueDate == null || ret.Lastdate <= model.EndDueDate.Value)
                                            && (model.StartPeriod == null || ret.Ryear >= model.StartPeriod.Value.Year)
                                            && (model.EndPeriod == null || ret.Ryear <= model.EndPeriod.Value.Year)

                                         orderby ret.Lastdate ascending
                                         select new { ret, retemp, loc, org }).ToListAsync();



                    return returns.Select(x => new ComplianceViewModel
                    {
                        Id = x.ret.Rtid,
                        Oid = x.org.Oid,
                        Lcode = x.loc.Lcode,
                        Oname = x.org.Oname,
                        Lname = x.loc.Lname,
                        Lcity = x.loc.Lcity,
                        Lstate = x.loc.Lstate,
                        ServiceType = "Returns",
                        Service = x.retemp.Rtitle,
                        Period = x.ret.Ryear.ToString(),
                        DueDate = x.ret.Lastdate.HasValue ? x.ret.Lastdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        CompletionDate = x.ret.Depdate.HasValue ? x.ret.Depdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        Status = StatusDescriptions.TryGetValue(x.ret.Status ?? -1, out var description) ? description : "Unknown",
                        FileName = x.ret.Filename

                    }).ToList();
                }
            }
            else {
                var uno = _httpContext.Session.GetInt32("UNO");

                if (model == null)
                {

                    var returns = await (from ret in _context.Ncrets
                                         join retemp in _context.Nctemprets on ret.Rcode equals retemp.Rcode
                                         join loc in _context.Ncmlocs on ret.Lcode equals loc.Lcode
                                         join org in _context.Ncmorgs on loc.Oid equals org.Oid
                                         join usem in _context.Ncumaps on new { loc.Oid, loc.Lcode } equals new { usem.Oid, usem.Lcode }
                                         where ret.Ryear == DateTime.Now.Year
                                         where usem.Uno == uno && loc.Lactive == 1 && org.Oactive == 1
                                         orderby ret.Lastdate ascending
                                         select new { ret, retemp, loc, org }).ToListAsync();
                    return returns.Select(x => new ComplianceViewModel
                    {
                        Id = x.ret.Rtid,
                        Oid = x.org.Oid,
                        Lcode = x.loc.Lcode,
                        Oname = x.org.Oname,
                        Lname = x.loc.Lname,
                        Lcity = x.loc.Lcity,
                        Lstate = x.loc.Lstate,
                        ServiceType = "Returns",
                        Service = x.retemp.Rtitle,
                        Period = x.ret.Ryear.ToString(),
                        DueDate = x.ret.Lastdate.HasValue ? x.ret.Lastdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        CompletionDate = x.ret.Depdate.HasValue ? x.ret.Depdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        Status = StatusDescriptions.TryGetValue(x.ret.Status ?? -1, out var description) ? description : "Unknown",
                        FileName = x.ret.Filename

                    }).ToList();
                }
                else
                {
                    var returns = await (from ret in _context.Ncrets
                                         join retemp in _context.Nctemprets on ret.Rcode equals retemp.Rcode
                                         join loc in _context.Ncmlocs on ret.Lcode equals loc.Lcode
                                         join org in _context.Ncmorgs on loc.Oid equals org.Oid
                                         join usem in _context.Ncumaps on new { loc.Oid, loc.Lcode } equals new { usem.Oid, usem.Lcode }
                                         where usem.Uno == uno && loc.Lactive == 1 && org.Oactive == 1
                                            && (string.IsNullOrEmpty(model.SelectedClient) || org.Oid == model.SelectedClient)
                                            && (string.IsNullOrEmpty(model.SelectedSite) || loc.Lcode == model.SelectedSite)
                                            && (string.IsNullOrEmpty(model.SelectedState) || loc.Lstate == model.SelectedState)
                                            && (string.IsNullOrEmpty(model.SelectedCity) || loc.Lcity == model.SelectedCity)
                                            && (model.StartDueDate == null || ret.Lastdate >= model.StartDueDate.Value)
                                            && (model.EndDueDate == null || ret.Lastdate <= model.EndDueDate.Value)
                                            && (model.StartPeriod == null || ret.Ryear >= model.StartPeriod.Value.Year)
                                            && (model.EndPeriod == null || ret.Ryear <= model.EndPeriod.Value.Year)

                                         orderby ret.Lastdate ascending
                                         select new { ret, retemp, loc, org }).ToListAsync();



                    return returns.Select(x => new ComplianceViewModel
                    {
                        Id = x.ret.Rtid,
                        Oid = x.org.Oid,
                        Lcode = x.loc.Lcode,
                        Oname = x.org.Oname,
                        Lname = x.loc.Lname,
                        Lcity = x.loc.Lcity,
                        Lstate = x.loc.Lstate,
                        ServiceType = "Returns",
                        Service = x.retemp.Rtitle,
                        Period = x.ret.Ryear.ToString(),
                        DueDate = x.ret.Lastdate.HasValue ? x.ret.Lastdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        CompletionDate = x.ret.Depdate.HasValue ? x.ret.Depdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        Status = StatusDescriptions.TryGetValue(x.ret.Status ?? -1, out var description) ? description : "Unknown",
                        FileName = x.ret.Filename

                    }).ToList();
                }
            }

        }

        public async Task<List<string>> GetOrganizationNamesAsync(int uno)     // code for getting oname on basis of uno and oid in filters)

        {
            var sqlQuery = @" SELECT DISTINCT d.OName
                                 FROM NCUMAP m
                                 JOIN NCMORG d ON m.Oid = d.Oid
                                 WHERE m.Uno = {0} AND d.OActive = 1";

            var organizationNames = await _context.Ncmorgs

           .FromSqlRaw(sqlQuery, uno)
           .Select(o => o.Oname ?? string.Empty)
           .Where(o => !string.IsNullOrEmpty(o))
           .ToListAsync();

            return organizationNames;

        }

        public async Task<List<string>> GetLocationNamesAsync(int uno)
        {
            var sqlQuery = @" SELECT DISTINCT b.Lname
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

        public async Task<List<string>> GetFilteredLocationNamesAsync(int uno, string organizationName)
        {
            var sqlQuery = @"
                SELECT DISTINCT b.Lname
                FROM NCUMAP m
                JOIN NCMLOC b ON m.Lcode = b.Lcode
                JOIN NCMORG d ON m.Oid = d.Oid
                WHERE m.Uno = @uno 
                AND b.Lactive = 1
                AND d.OActive = 1
                AND d.OName = @organizationName";

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
            var sqlQuery = @"   SELECT DISTINCT e.statedesc
                                FROM NCUMAP m
                                JOIN NCMLOC b ON m.Lcode = b.Lcode
                                JOIN MASTSTATES e ON b.lstate = e.stateid
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

        public async Task<Ncret> GetByIdAsync(int rtid, string oid, string lcode)
        {
            var returns = await _context.Ncrets
         .FirstOrDefaultAsync(r => r.Rtid == rtid && r.Oid == oid && r.Lcode == lcode);

            if (returns == null)
            {
                Console.WriteLine("⚠️ No record found in database!");
            }

            return returns;
        }


        public async Task<string> UpdateRetAsync(int? rtid, string oid, string lcode, int status, DateOnly? deptdate, DateOnly? lastdate, string remarks, IFormFile file)
        {
            try
            {
                var ncretRecord = rtid.HasValue
                    ? await _context.Ncrets.FirstOrDefaultAsync(n => n.Rtid == rtid && n.Oid == oid && n.Lcode == lcode)
                    : new Ncret();

                if (ncretRecord == null && rtid.HasValue)
                {
                    return "Record not found.";
                }

                ncretRecord.Oid = oid;
                ncretRecord.Lcode = lcode;
                ncretRecord.Status = status;
                ncretRecord.Depdate = deptdate;
                ncretRecord.Lastdate = lastdate;
                ncretRecord.Remarks = remarks;
                

             

                if (file != null && file.Length > 0)
                {
                    if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                    {
                        return "Only PDF files are allowed.";
                    }

                    //string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid.ToString(), "RET");
                    //Directory.CreateDirectory(folderPath);

                    //string filePath = Path.Combine(folderPath, file.FileName);
                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await file.CopyToAsync(stream);
                    //}
                    string originalFileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(originalFileName);
                    string nameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                    if (originalFileName.Length > 50)
                    {
                        return "Filename too long. Must be 50 characters or fewer.";
                    }


                    string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid.ToString(), "RET");
                    Directory.CreateDirectory(folderPath);

                    string filePath = Path.Combine(folderPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    ncretRecord.Filename = file.FileName;
                }

                // Insert or update record
                if (!rtid.HasValue)
                {
                    _context.Ncrets.Add(ncretRecord);
                }
                else
                {
                    _context.Ncrets.Update(ncretRecord);
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
