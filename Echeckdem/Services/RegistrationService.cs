using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZstdSharp.Unsafe;

namespace Echeckdem.Services
{
    public class RegistrationService
    {
        private readonly DbEcheckContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RegistrationService(DbEcheckContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<List<RegistrationViewModel>> GetDataAsync( int ulev, int uno, string organizationName = null, string LocationName = null, string StateName = null, string CityName = null, DateOnly? StartDueDate = null, DateOnly? EndDueDate = null, DateOnly? StartPeriod = null, DateOnly? EndPeriod = null)
        {
            var currentYear = DateTime.Now.Year;

            var sqlQuery = @"
            SELECT a.oid, a.doe, a.status, a.Dolr, a.tp, a.doi, a.lcode, a.nmoe, a.noe, a.remarks, a.rno, a.uid,  
            b.lname, b.lstate, b.lcity, b.lregion,
            c.oname,
            d.statedesc as State

            FROM ncreg a
            JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
            JOIN ncmorg c ON c.oid = b.oid
            JOIN MASTSTATES d ON b.lstate = d.stateid
            WHERE c.oactive = 1 
            AND b.oid = a.oid
            AND a.lcode = b.lcode ";

            

            // Apply year condition only if no filters are specified
            if (string.IsNullOrEmpty(organizationName) &&
                string.IsNullOrEmpty(LocationName) &&
                string.IsNullOrEmpty(StateName) &&
                string.IsNullOrEmpty(CityName))
            {
                sqlQuery += "AND YEAR(a.doe) = @currentYear ";
            }

            if (ulev >= 1)
            {
                sqlQuery += @" 
                AND b.oid IN (SELECT DISTINCT oid FROM ncumap WHERE uno = @uno)
                AND b.lactive = '1'
                AND a.lcode IN (SELECT DISTINCT lcode FROM ncumap WHERE uno = @uno) ";
            }

            // Applying filters

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
                sqlQuery += " AND a.doe >= @StartDueDate";
            }
            if (EndDueDate.HasValue)
            {
                sqlQuery += "AND a.doe <= @EndDueDate";
            }    
            if (StartPeriod.HasValue)
            {
                sqlQuery += " ";
            }
            if (EndPeriod.HasValue)
            {
                sqlQuery += " "; 
            }
            sqlQuery += " ORDER BY a.doe DESC, b.lname";

            var result = await _context.RegistrationViewModel
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
            var sqlQuery = @" 
            SELECT DISTINCT c.OName
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

                    string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid.ToString(), "REG");
                    Directory.CreateDirectory(folderPath);

                    string filePath = Path.Combine(folderPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    ncregRecord.Filename = file.FileName;
                }

                // Insert or update record
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
