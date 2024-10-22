using Echeckdem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using System.Security.Policy;

namespace Echeckdem.Services
{
    public class ContributionService
    {
        private readonly DbEcheckContext _context;

        public ContributionService(DbEcheckContext context)
        {
            _context = context;
        }

        public async Task<List<ContributionViewModel>> GetDataAsync(int ulev,string uno, string organizationName = null, string site = null, string state = null, string city = null)
        {
            var sqlQuery = @"
                                SELECT a.oid, a.tp, a.Status, a.depdate, a.Period, a.Cyear, a.lastdate,
                                b.lname, b.lstate, b.lcity, b.lregion, 
                                c.oname

                         FROM nccontr a
            JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
            JOIN ncmorg c ON c.oid = b.oid
            WHERE c.oactive = 1 AND a.status <> 99";

            // Add extra filtering if ulev > 1
            if (ulev > 1)
            {
                sqlQuery += @"
                AND a.oid IN (SELECT DISTINCT oid FROM ncumap WHERE uno = {0})
                AND a.lcode IN (SELECT DISTINCT lcode FROM ncumap WHERE uno = {0})
                AND b.lactive = '1'";
            }

            //Applyting FILTERSS

            if (!string.IsNullOrEmpty(organizationName))
            {
                sqlQuery += " AND c.oname LIKE '%' + {1} + '%'";
            }
            if (!string.IsNullOrEmpty(site))
            {
                sqlQuery += " AND b.lname LIKE '%' + {2} + '%'";
            }
            if (!string.IsNullOrEmpty(city))
            {
                sqlQuery += " AND b.lcity LIKE '%' + {4} + '%'";
            }
            if (!string.IsNullOrEmpty(state))
            {
                sqlQuery += " AND b.lstate LIKE '%' + {3} + '%'";
            }
           


            sqlQuery += @" ORDER BY a.lastdate DESC, b.lname";

            // Execute the SQL query
            var result = await _context.ContributionViewModel.FromSqlRaw(sqlQuery, uno, organizationName, site, state, city).ToListAsync();
            return result;
        }

    }
}
