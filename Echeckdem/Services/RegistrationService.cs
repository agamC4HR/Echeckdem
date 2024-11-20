using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Echeckdem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Services
{
    public class RegistrationService
    {
        private readonly DbEcheckContext _context;

        public RegistrationService(DbEcheckContext context)
        {
            _context = context;
        }
        public async Task<List<RegistrationViewModel>> GetDataAsync( int ulev, int uno, string organizationName = null, string LocationName = null, string StateName = null, string CityName = null)
        {
            var currentYear = DateTime.Now.Year;

            var sqlQuery = @"
        SELECT a.oid, a.doe, a.status, a.Dolr, a.tp,  
        b.lname, b.lstate, b.lcity, b.lregion,
        c.oname,
        d.statedesc as State

        FROM ncreg a
        JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
        JOIN ncmorg c ON b.oid = c.oid
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
            AND a.lcode IN (SELECT DISTINCT lcode FROM ncumap WHERE uno = @uno)";
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
                sqlQuery += " AND d.statedesc = @StateName";//sqlQuery += " AND b.lstate = @StateName";
            }

            if (!string.IsNullOrEmpty(CityName))
            {
                sqlQuery += " AND b.lcity = @CityName";
            }

            sqlQuery += " ORDER BY a.doe DESC, b.lname";

            var result = await _context.RegistrationViewModel
                .FromSqlRaw(sqlQuery,
                    new SqlParameter("@currentYear", currentYear),
                    new SqlParameter("@uno", uno),
                    new SqlParameter("@organizationName", (object)organizationName ?? DBNull.Value),
                    new SqlParameter("@LocationName", (object)LocationName ?? DBNull.Value),
                    new SqlParameter("@StateName", (object)StateName ?? DBNull.Value),
                    new SqlParameter("@CityName", (object)CityName ?? DBNull.Value))
                .ToListAsync();

            return result;
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
                            JOIN MASTSTATES c ON b.lstate = c.STATEID
                            WHERE m.Uno = {0} AND b.Lactive = 1";

            //    var sqlQuery = @"
            //SELECT DISTINCT b.Lstate
            //FROM NCUMAP m
            //JOIN NCMLOC b ON m.Lcode = b.Lcode
            //WHERE m.Uno = {0} AND b.Lactive = 1";

            //var stateNames = await _context.Ncmlocs
            //    .FromSqlRaw(sqlQuery, uno)
            //    .Select(s => s.Lstate ?? string.Empty)
            //    .Where(s => !string.IsNullOrEmpty(s))
            //    .ToListAsync();
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
    }
}
