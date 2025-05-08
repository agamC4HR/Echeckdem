using Echeckdem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Services
{
    public class BOCWService
    {
        private readonly DbEcheckContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BOCWService(DbEcheckContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<BocwViewModel>> GetDataAsync(int ulev, int uno, string organizationName = null, string LocationName = null, string StateName = null, string CityName = null, DateOnly? StartDueDate = null, DateOnly? EndDueDate = null, DateOnly? StartPeriod = null, DateOnly? EndPeriod = null)
        {

            var currentYear = DateTime.Now.Year;
            var sqlQuery = @"
                                SELECT a.lcode, a.lname, a.ProjectCode, a.ScopeID, a.DueDate, a.Status, a.Task, a.CreateDate, a.TransactionID,
                                b.lstate, b.lcity, b.lregion,
                                c.oname,                
                                d.statedesc as State

                                FROM ncbocw a
                                JOIN ncmloc b ON a.lcode = b.lcode
                                JOIN ncmorg c ON c.oid = b.oid
                                JOIN MASTSTATES d ON b.lstate = d.stateid
                                WHERE c.oactive = 1 
                                
                                AND a.lcode = b.lcode
                                AND (a.status IS NULL OR a.status <> 99) ";
           
            if (string.IsNullOrEmpty(organizationName) &&
                string.IsNullOrEmpty(LocationName) &&
                string.IsNullOrEmpty(StateName) &&
                string.IsNullOrEmpty(CityName))
            {
                sqlQuery += "AND YEAR(a.DueDate) = @currentYear ";
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
                sqlQuery += " AND a.lname = @LocationName";
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
                sqlQuery += " AND a.DueDate >= @StartDueDate";
            }
            if (EndDueDate.HasValue)
            {
                sqlQuery += " AND a.DueDate <= @EndDueDate";
            }
            if (StartPeriod.HasValue)
            {
                sqlQuery += " AND a.Period >= @StartPeriod";
            }
            if (EndPeriod.HasValue)
            {
                sqlQuery += " AND a.Period <= @EndPeriod";
            }

            sqlQuery += " ORDER BY a.DueDate DESC, a.lname";

            // Execute the SQL query 
            var result = await _context.BocwViewModel
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

        public async Task<Ncbocw> GetbyIdAsync(int transactionId, string lcode)
        {
            var bocw = await _context.Ncbocws.FirstOrDefaultAsync(b=> b.TransactionId == transactionId && b.Lcode == lcode);
            if(bocw == null)
            {
                Console.WriteLine("No crecord found under BOCW ");
            }
            return bocw;           
        }


       // public async Task 


    }
}
