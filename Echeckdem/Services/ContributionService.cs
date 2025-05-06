using Echeckdem.Models;
using Echeckdem.CustomFolder.DetailViewDashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using System.Security.Policy;

namespace Echeckdem.Services
{
    public class ContributionService
    {
        private readonly DbEcheckContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContributionService(DbEcheckContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<ContributionViewModel>> GetDataAsync(int ulev, int uno, string organizationName = null, string LocationName = null, string StateName = null, string CityName = null, DateOnly? StartDueDate = null, DateOnly? EndDueDate = null, DateOnly? StartPeriod = null, DateOnly? EndPeriod = null)
        {

            var currentYear = DateTime.Now.Year;
            var sqlQuery = @"
                                SELECT a.oid, a.tp, a.Status, a.depdate, a.Period, a.Cyear, a.lastdate, a.contid, a.lcode, a.amount, a.chqdate, a.chqno, a.remarks, a.Filename,
                                b.lname, b.lstate, b.lcity, b.lregion, 
                                c.oname,
                                d.statedesc as State

                                FROM nccontr a
                                JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
                                JOIN ncmorg c ON c.oid = b.oid
                                JOIN MASTSTATES d ON b.lstate = d.stateid
                                WHERE c.oactive = 1 
                                AND b.oid = a.oid
                                AND a.lcode = b.lcode
                                AND (a.status IS NULL OR a.status <> 99) ";
            //AND YEAR(a.lastdate) = @currentYear";
            //AND a.status <> 99
            if (string.IsNullOrEmpty(organizationName) &&
                string.IsNullOrEmpty(LocationName) &&
                string.IsNullOrEmpty(StateName) &&
                string.IsNullOrEmpty(CityName))
            {
                sqlQuery += "AND YEAR(a.lastdate) = @currentYear ";
            }
            // Add extra filtering if ulev > 1 
            if (ulev >= 1)
            {
                sqlQuery += @" AND b.oid IN (SELECT DISTINCT oid FROM ncumap WHERE uno = @uno)
                           AND b.lactive = '1'
                           AND a.lcode IN (SELECT DISTINCT lcode FROM ncumap WHERE uno = @uno)";
            }
            // applying filters
            if (!string.IsNullOrEmpty(organizationName))
            {
                sqlQuery += " AND c.oname = @organizationName";
            }
            if (!string.IsNullOrEmpty(LocationName))
            {
                sqlQuery += " AND b.lname = @LocationName";
            }

            if (!string.IsNullOrEmpty(StateName))
            {
                sqlQuery += " AND d.statedesc = @StateName";
            }

            if (!string.IsNullOrEmpty(CityName))
            {
                sqlQuery += " AND b.lcity = @CityName";
            }
            if (StartDueDate.HasValue)
            {
                sqlQuery += " AND a.lastdate >= @StartDueDate";
            }
            if (EndDueDate.HasValue)
            {
                sqlQuery += " AND a.lastdate <= @EndDueDate";
            }
            if (StartPeriod.HasValue)
            {
                sqlQuery += " AND a.Period >= @StartPeriod";
            }
            if (EndPeriod.HasValue)
            {
                sqlQuery += " AND a.Period <= @EndPeriod";
            }

            sqlQuery += " ORDER BY a.lastdate DESC, b.lname";

            // Execute the SQL query 
            var result = await _context.ContributionViewModel
                 .FromSqlRaw(sqlQuery,
                             new SqlParameter("@currentYear", currentYear),
                             new SqlParameter("@uno", uno),
                             new SqlParameter("@organizationName", (object)organizationName ?? DBNull.Value),
                             new SqlParameter("@LocationName", (object)LocationName ?? DBNull.Value),
                             new SqlParameter("@StateName", (object)StateName ?? DBNull.Value),
                             new SqlParameter("@CityName", (object)CityName ?? DBNull.Value),
                             new SqlParameter("@StartDueDate", (object)StartDueDate ?? DBNull.Value),
                             new SqlParameter("@EndDueDate", (object)EndDueDate ?? DBNull.Value),
                             new SqlParameter("@StartPeriod", (object)StartPeriod ?? DBNull.Value),
                             new SqlParameter("@EndPeriod", (object)EndPeriod ?? DBNull.Value))
                .ToListAsync();
            return result;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync (int uno)
        {
            var currentYear = DateTime.Now.Year;

            var allowedLcodes = await _context.Ncumaps
           .Where(m => m.Uno == uno)
           .Select(m => m.Lcode)
           .ToListAsync();

            var onTimeContributions = await _context.Nccontrs
                .Where(c => allowedLcodes.Contains(c.Lcode) &&
                        c.Cyear == currentYear &&
                        c.Status == 1 &&
                        c.Depdate <= c.Lastdate)
                .Join(_context.Ncmlocs, contr => contr.Lcode, loc => loc.Lcode, (contr, loc) => new ContributionViewModel
                {
                    Lcode = contr.Lcode,
                    Depdate = contr.Depdate,
                    LastDate = contr.Lastdate,
                    Lname = loc.Lname,
                    Status = contr.Status,
                    Cyear = contr.Cyear,
                    Period = contr.Period,
                    Remarks = contr.Remarks,
                    Amount = contr.Amount,
                    Chqno = contr.Chqno
                })
                .ToListAsync();

            return new DashboardViewModel
            {
                OnTimeContributions = onTimeContributions,
                OnTimeCount = onTimeContributions.Count
            };
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

                    string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid.ToString(), "CONTR");
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
