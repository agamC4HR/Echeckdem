using Echeckdem.Models;
using Echeckdem.CustomFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mono.TextTemplating;
using System.Security.Policy;

namespace Echeckdem.Services
{
    public class ReturnsService
    {
        private readonly DbEcheckContext _context;

        public ReturnsService(DbEcheckContext context)
        {
            _context = context;
        }

        public async Task<List<ReturnsViewModel>> GetDataAsync(int ulev, int uno, string organizationName = null, string LocationName = null, string StateName = null, string CityName = null)
        {
            var currentYear = DateTime.Now.Year;

            var sqlQuery = @"
                                SELECT a.oid, a.Depdate, a.Status, 
                                b.lname, b.lstate, b.lcity, b.lregion, 
                                c.rtitle, c.rform, c.RM, c.YROFF, 
                                d.oname,
                                a.lastdate
                                

                                FROM ncret a
                                JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
                                JOIN nctempret c ON a.rcode = c.rcode
                                JOIN ncmorg d ON b.oid = d.oid  
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
                sqlQuery += "AND d.oname = @organizationName";
            }
            if (!string.IsNullOrEmpty(LocationName))
            {
                sqlQuery += "AND b.lname = @LocationName";
            }

            if (!string.IsNullOrEmpty(StateName))
            {
                sqlQuery += " AND b.lstate = @StateName";
            }

            if (!string.IsNullOrEmpty(CityName))
            {
                sqlQuery += " AND b.lcity = @CityName";
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
                             new SqlParameter("@CityName", (object)CityName ?? DBNull.Value))
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
            var sqlQuery = @"
        SELECT DISTINCT b.Lstate
        FROM NCUMAP m
        JOIN NCMLOC b ON m.Lcode = b.Lcode
        WHERE m.Uno = {0} AND b.Lactive = 1";

            var stateNames = await _context.Ncmlocs
                .FromSqlRaw(sqlQuery, uno)
                .Select(s => s.Lstate ?? string.Empty)
                .Where(s => !string.IsNullOrEmpty(s))
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
