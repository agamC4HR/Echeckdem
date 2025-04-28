using Echeckdem.Models;
using Echeckdem.CustomFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mono.TextTemplating;
using System.Security.Policy;
using Microsoft.AspNetCore.Hosting;

namespace Echeckdem.Services
{
    public class ReturnsService
    {
        private readonly DbEcheckContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReturnsService(DbEcheckContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<ReturnsViewModel>> GetDataAsync(int ulev, int uno, string organizationName = null, string LocationName = null, string StateName = null, string CityName = null, DateOnly? StartDueDate = null, DateOnly? EndDueDate = null, DateOnly? StartPeriod = null, DateOnly? EndPeriod = null)
        {
            var currentYear = DateTime.Now.Year;

            var sqlQuery = @"
                                SELECT a.oid, a.Depdate, a.Status, a.lastdate, a.remarks, a.rtid, a.lcode,
                                b.lname, b.lstate, b.lcity, b.lregion, 
                                c.rtitle, c.rform, c.RM, c.YROFF, 
                                d.oname,
                                e.statedesc as State
                                

                                FROM ncret a
                                JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
                                JOIN nctempret c ON a.rcode = c.rcode
                                JOIN ncmorg d ON b.oid = d.oid  
                                JOIN MASTSTATES e ON b.lstate = e.stateid
                                WHERE d.oactive = 1 
                                AND a.status <> 99";
            

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

            //Applyting FILTERSS

            if (!string.IsNullOrEmpty(organizationName))
            {
                sqlQuery += " AND d.oname = @organizationName";
            }
            if (!string.IsNullOrEmpty(LocationName))
            {
                sqlQuery += " AND b.lname = @LocationName";
            }

            if (!string.IsNullOrEmpty(StateName))
            {
                sqlQuery += " AND e.statedesc = @StateName";
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
                sqlQuery += " AND a.Depdate >= @StartPeriod";
            }
            if (EndPeriod.HasValue)
            {
                sqlQuery += " AND a.Depdate <= @EndPeriod";
            }

            sqlQuery += " ORDER BY a.lastdate DESC, b.lname";

            // Execute the SQL query
            var result = await _context.ReturnsViewModel
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
