using Echeckdem.Models;
using Echeckdem.CustomFolder.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using System.Security.Policy;

using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using Echeckdem.ViewModel.Shared;
using Echeckdem.ViewModel.ComplianceTracker;
namespace Echeckdem.Services
{
    public class ContributionService
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

        public ContributionService(DbEcheckContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<List<ComplianceViewModel>> GetContributionAsync( FilterFormModel model = null)
        {
            if (_httpContext.Session.GetInt32("User Level") == 1)
            {
                if (model == null)
                {
                    var contribution = await (from pay in _context.Nccontrs
                                              join loc in _context.Ncmlocs on pay.Lcode equals loc.Lcode
                                              join org in _context.Ncmorgs on loc.Oid equals org.Oid
                                              
                                              where loc.Lactive == 1 && org.Oactive == 1
                                              orderby pay.Lastdate ascending
                                              select new { pay, loc, org }).ToListAsync();
                    return contribution.Select(x => new ComplianceViewModel
                    {
                        Id = x.pay.Contid,
                        Oid = x.org.Oid,
                        Lcode = x.loc.Lcode,
                        Oname = x.org.Oname,
                        Lname = x.loc.Lname,
                        Lcity = x.loc.Lcity,
                        Lstate = x.loc.Lstate,
                        ServiceType = "Payroll",
                        Service = x.pay.Tp,
                        Period = x.pay.Period.ToString(),
                        DueDate = x.pay.Lastdate.HasValue ? x.pay.Lastdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        CompletionDate = x.pay.Depdate.HasValue ? x.pay.Depdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        Status = StatusDescriptions.TryGetValue(x.pay.Status ?? -1, out var description) ? description : "Unknown",
                        FileName = x.pay.Filename

                    }).ToList();
                }
                else
                {
                    var contribution = await (from pay in _context.Nccontrs
                                              join loc in _context.Ncmlocs on pay.Lcode equals loc.Lcode
                                              join org in _context.Ncmorgs on loc.Oid equals org.Oid
                                              
                                              where loc.Lactive == 1 && org.Oactive == 1
                                                && (string.IsNullOrEmpty(model.SelectedClient) || org.Oid == model.SelectedClient)
                                                && (string.IsNullOrEmpty(model.SelectedSite) || loc.Lcode == model.SelectedSite)
                                                && (string.IsNullOrEmpty(model.SelectedState) || loc.Lstate == model.SelectedState)
                                                && (string.IsNullOrEmpty(model.SelectedCity) || loc.Lcity == model.SelectedCity)
                                                && (model.StartDueDate == null || pay.Lastdate >= model.StartDueDate.Value)
                                                && (model.EndDueDate == null || pay.Lastdate <= model.EndDueDate.Value)
                                                && (model.StartPeriod == null || (pay.Period >= model.StartPeriod.Value.Month && pay.Cyear >= model.StartPeriod.Value.Year))
                                                && (model.EndPeriod == null || (pay.Period <= model.EndPeriod.Value.Month && pay.Cyear <= model.EndPeriod.Value.Year))
                                              orderby pay.Lastdate ascending
                                              select new { pay, loc, org }).ToListAsync();
                    return contribution.Select(x => new ComplianceViewModel
                    {
                        Id = x.pay.Contid,
                        Oid = x.org.Oid,
                        Lcode = x.loc.Lcode,
                        Oname = x.org.Oname,
                        Lname = x.loc.Lname,
                        Lcity = x.loc.Lcity,
                        Lstate = x.loc.Lstate,
                        ServiceType = "Payroll",
                        Service = x.pay.Tp,
                        Period = x.pay.Period.ToString(),
                        DueDate = x.pay.Lastdate.HasValue ? x.pay.Lastdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        CompletionDate = x.pay.Depdate.HasValue ? x.pay.Depdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        Status = StatusDescriptions.TryGetValue(x.pay.Status ?? -1, out var description) ? description : "Unknown",
                        FileName = x.pay.Filename

                    }).ToList();
                }
            }
            else 
            {
                var uno = _httpContext.Session.GetInt32("UNO");

                if (model == null)
                {
                    var contribution = await (from pay in _context.Nccontrs
                                              join loc in _context.Ncmlocs on pay.Lcode equals loc.Lcode
                                              join org in _context.Ncmorgs on loc.Oid equals org.Oid
                                              join usem in _context.Ncumaps on new { loc.Oid, loc.Lcode } equals new { usem.Oid, usem.Lcode }
                                              where usem.Uno == uno && loc.Lactive == 1 && org.Oactive == 1
                                              orderby pay.Lastdate ascending
                                              select new { pay, loc, org }).ToListAsync();
                    return contribution.Select(x => new ComplianceViewModel
                    {
                        Id = x.pay.Contid,
                        Oid = x.org.Oid,
                        Lcode = x.loc.Lcode,
                        Oname = x.org.Oname,
                        Lname = x.loc.Lname,
                        Lcity = x.loc.Lcity,
                        Lstate = x.loc.Lstate,
                        ServiceType = "Payroll",
                        Service = x.pay.Tp,
                        Period = x.pay.Period.ToString(),
                        DueDate = x.pay.Lastdate.HasValue ? x.pay.Lastdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        CompletionDate = x.pay.Depdate.HasValue ? x.pay.Depdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        Status = StatusDescriptions.TryGetValue(x.pay.Status ?? -1, out var description) ? description : "Unknown",
                        FileName = x.pay.Filename

                    }).ToList();
                }
                else
                {
                    var contribution = await (from pay in _context.Nccontrs
                                              join loc in _context.Ncmlocs on pay.Lcode equals loc.Lcode
                                              join org in _context.Ncmorgs on loc.Oid equals org.Oid
                                              join usem in _context.Ncumaps on new { loc.Oid, loc.Lcode } equals new { usem.Oid, usem.Lcode }
                                              where usem.Uno == uno && loc.Lactive == 1 && org.Oactive == 1
                                                && (string.IsNullOrEmpty(model.SelectedClient) || org.Oid == model.SelectedClient)
                                                && (string.IsNullOrEmpty(model.SelectedSite) || loc.Lcode == model.SelectedSite)
                                                && (string.IsNullOrEmpty(model.SelectedState) || loc.Lstate == model.SelectedState)
                                                && (string.IsNullOrEmpty(model.SelectedCity) || loc.Lcity == model.SelectedCity)
                                                && (model.StartDueDate == null || pay.Lastdate >= model.StartDueDate.Value)
                                                && (model.EndDueDate == null || pay.Lastdate <= model.EndDueDate.Value)
                                                && (model.StartPeriod == null || (pay.Period >= model.StartPeriod.Value.Month && pay.Cyear >= model.StartPeriod.Value.Year))
                                                && (model.EndPeriod == null || (pay.Period <= model.EndPeriod.Value.Month && pay.Cyear <= model.EndPeriod.Value.Year))
                                              orderby pay.Lastdate ascending
                                              select new { pay, loc, org }).ToListAsync();
                    return contribution.Select(x => new ComplianceViewModel
                    {
                        Id = x.pay.Contid,
                        Oid = x.org.Oid,
                        Lcode = x.loc.Lcode,
                        Oname = x.org.Oname,
                        Lname = x.loc.Lname,
                        Lcity = x.loc.Lcity,
                        Lstate = x.loc.Lstate,
                        ServiceType = "Payroll",
                        Service = x.pay.Tp,
                        Period = x.pay.Period.ToString(),
                        DueDate = x.pay.Lastdate.HasValue ? x.pay.Lastdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        CompletionDate = x.pay.Depdate.HasValue ? x.pay.Depdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                        Status = StatusDescriptions.TryGetValue(x.pay.Status ?? -1, out var description) ? description : "Unknown",
                        FileName = x.pay.Filename

                    }).ToList();
                }
            }
                

        }

       public async Task<List<string>> GetOrganizationNamesAsync(int uno)     // code for getting oname on basis of uno and oid in filters)

        {
            var sqlQuery = @" SELECT DISTINCT c.OName
                                 FROM NCUMAP m
                                 JOIN NCMORG c ON m.Oid = c.Oid
                                 WHERE m.Uno = {0} AND c.OActive = 1";

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
            var sqlQuery = @"SELECT DISTINCT c.Statedesc
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


        public async Task<Nccontr> GetByIdAsync(int contid, string oid, string lcode)
        {
            var Contribution = await _context.Nccontrs
         .FirstOrDefaultAsync(r => r.Contid == contid && r.Oid == oid && r.Lcode == lcode);

            if (Contribution == null)
            {
                Console.WriteLine("⚠️ No record found in database!");
            }

            return Contribution;
        }


        public async Task<string> UpdateContrAsync(int? contid, string oid, string lcode, int status, string amount , string chqno, DateOnly? chqdate, DateOnly? deptdate, DateOnly? lastdate, string remarks, IFormFile file)
        {
            try
            {
                var contrRecord = contid.HasValue
                    ? await _context.Nccontrs.FirstOrDefaultAsync(n => n.Contid == contid && n.Oid == oid && n.Lcode == lcode)
                    : new Nccontr();

                if (contrRecord == null && contid.HasValue)
                {
                    return "Record not found.";
                }

                contrRecord.Oid = oid;
                contrRecord.Lcode = lcode;
                contrRecord.Status = status;
                contrRecord.Amount = amount;
                contrRecord.Chqno = chqno;
                contrRecord.Chqdate = chqdate;
                contrRecord.Depdate = deptdate;
                contrRecord.Lastdate = lastdate;
                contrRecord.Remarks = remarks;

                if (file != null && file.Length > 0)
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


                    string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid.ToString(), "RET");
                    Directory.CreateDirectory(folderPath);

                    string filePath = Path.Combine(folderPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    contrRecord.Filename = file.FileName;
                }

                // Insert or update record
                if (!contid.HasValue)
                {
                    _context.Nccontrs.Add(contrRecord);
                }
                else
                {
                    _context.Nccontrs.Update(contrRecord);
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
