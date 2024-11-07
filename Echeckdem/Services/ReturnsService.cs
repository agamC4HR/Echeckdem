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

        public async Task<List<ReturnsViewModel>> GetDataAsync(int ulev, string uno, string organizationName = null, string site = null, string state = null, string city = null, DateTime? startDueDate = null, DateTime? endDueDate = null, DateTime? startPeriod = null, DateTime? endPeriod = null)
        {
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

            // Add extra filtering if ulev > 1
            if (ulev > 1)
            {
                sqlQuery += @" AND b.oid IN (SELECT DISTINCT oid FROM ncumap WHERE uno = {0})
                           AND b.lactive = '1'
                           AND a.lcode IN (SELECT DISTINCT lcode FROM ncumap WHERE uno = {0})";
            }

            //Applyting FILTERSS

            if (!string.IsNullOrEmpty(organizationName))
            {
                sqlQuery += " AND d.oname LIKE '%' + {1} + '%'";
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

            if (startDueDate.HasValue)
            {
                sqlQuery += " AND a.lastdate >= {5}";
            }
            if (endDueDate.HasValue)
            {
                sqlQuery += " AND a.lastdate <= {6}";
            }
            if (startPeriod.HasValue)
            {
                //sqlQuery += " AND DATEADD(MONTH, a.RM, DATEADD(YEAR, a.YROFF, GETDATE())) >= {7}";
                //sqlQuery += " AND a.Period >= {7}";
            }
            if (endPeriod.HasValue)
            {
                //sqlQuery += " AND DATEADD(MONTH, a.RM, DATEADD(YEAR, a.YROFF, GETDATE())) <= {8}";
                //sqlQuery += " AND a.Period <= {8}";
            }

            sqlQuery += " ORDER BY a.lastdate DESC, b.lname";

            // Execute the SQL query
            var result = await _context.ReturnsViewModel.FromSqlRaw(sqlQuery, uno, organizationName, site, state, city, startDueDate, endDueDate, startPeriod, endPeriod).ToListAsync();
            return result;
        }





    }
}
