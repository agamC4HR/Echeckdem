using Echeckdem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Services
{
    public class ContributionService
    {
        private readonly DbEcheckContext _context;

        public ContributionService(DbEcheckContext context)
        {
            _context = context;
        }

        public async Task<List<ContributionViewModel>> GetDataAsync(int ulev,string uno, string OName = null, string Lname = null)
        {
            var sqlQuery = @"
                                SELECT a.oid, a.lastdate,
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

            sqlQuery += @" ORDER BY a.lastdate DESC, b.lname";

            // Execute the SQL query
            //var result = await _context.ContributionViewModel.FromSqlRaw(sqlQuery, uno).ToListAsync();
            var result = await _context.ContributionViewModel.FromSqlRaw(sqlQuery, new SqlParameter("@uno", uno)).ToListAsync();
            return result;
           

        }

    }
}
