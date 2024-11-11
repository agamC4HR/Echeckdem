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
        public async Task<List<RegistrationViewModel>> GetDataAsync(int ulev, string uno, string organizationName = null, string site = null, string state = null, string city = null, DateTime? startDueDate = null, DateTime? endDueDate = null, DateTime? startPeriod = null, DateTime? endPeriod = null)
        {

            var currentYear = DateTime.Now.Year;

            var sqlQuery = @"
                SELECT a.oid, a.doe, a.status, a.Dolr, a.tp,  
                b.lname, b.lstate, b.lcity, b.lregion,
                c.oname
                
                FROM ncreg a
                JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
                JOIN ncmorg c ON b.oid = c.oid
                WHERE c.oactive = 1 
                AND b.oid = a.oid
                AND a.lcode = b.lcode 
               AND YEAR(a.doe) = {0}";
                                                                                       // show  data of current year in home page of reg
            
            if (ulev > 1)

            {
                sqlQuery += @" AND b.oid IN (SELECT DISTINCT oid FROM ncumap WHERE uno = {1})
                           AND b.lactive = '1'
                           AND a.lcode IN (SELECT DISTINCT lcode FROM ncumap WHERE uno = {1})";
            }

            //Applyting FILTERSS

            if (!string.IsNullOrEmpty(organizationName))
            {
                sqlQuery += " AND c.oname LIKE '%' + {2} + '%'";
            }
            if (!string.IsNullOrEmpty(site))
            {
                sqlQuery += " AND b.lname LIKE '%' + {3} + '%'";
            }
            if (!string.IsNullOrEmpty(city))
            {
                sqlQuery += " AND b.lcity LIKE '%' + {4} + '%'";
            }
            if (!string.IsNullOrEmpty(state))
            {
                sqlQuery += " AND b.lstate LIKE '%' + {5} + '%'";
            }
            if (startDueDate.HasValue)
            {
                sqlQuery += " AND a.doe >= {6}";
            }
            if (endDueDate.HasValue)
            {
                sqlQuery += " AND a.doe <= {7}";
            }
            if (startPeriod.HasValue)
            {
                //sqlQuery += " AND a.Dolr >= {8}";
            }
            if (endPeriod.HasValue)
            {
                //sqlQuery += " AND a.Dolr <= {9}";
            }

            // 

            sqlQuery += " ORDER BY a.doe DESC, b.lname";

           
                
            //first return variable=
            var result = await _context.RegistrationViewModel.FromSqlRaw(sqlQuery, currentYear, uno, organizationName, site, state, city, startDueDate, endDueDate, startPeriod, endPeriod).ToListAsync();
            
            return result;
        }
    }
}
