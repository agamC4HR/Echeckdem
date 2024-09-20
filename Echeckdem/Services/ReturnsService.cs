using Echeckdem.Models;
using Echeckdem.CustomFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Echeckdem.Services
{
    public class ReturnsService
    {
        private readonly DbEcheckContext _context;

        public ReturnsService(DbEcheckContext context)
        {
            _context = context;
        }

        public async Task<List<ReturnsViewModel>> GetDataAsync(int ulev, string uno)
        {
           var sqlQuery = @"
                                SELECT a.oid, 
                                b.lname, b.lstate, b.lcity, b.lregion, 
                                c.rtitle, c.rform, 
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

            sqlQuery += " ORDER BY a.lastdate DESC, b.lname";

            // Execute the SQL query
            var result = await _context.ReturnsViewModel.FromSqlRaw(sqlQuery, uno).ToListAsync();

            return result;
                
        }





    }
}
