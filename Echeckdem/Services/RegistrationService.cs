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
        public async Task<List<RegistrationViewModel>> GetDataAsync(int ulev, string uno, string OName = null, string Lname = null)//[used for filtering in future])
        {
            var sqlQuery = @"
                SELECT a.oid, a.doe,
                b.lname, b.lstate, b.lcity, b.lregion,
                c.oname
                
                FROM ncreg a
                JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
                JOIN ncmorg c ON b.oid = c.oid
                WHERE c.oactive = 1 
                AND b.oid = a.oid
                AND a.lcode = b.lcode
            ";

            if (ulev > 1)

            {
                sqlQuery += @" AND b.oid IN (SELECT DISTINCT oid FROM ncumap WHERE uno = {0})
                           AND b.lactive = '1'
                           AND a.lcode IN (SELECT DISTINCT lcode FROM ncumap WHERE uno = {0})";
            }
            //{
            //    sqlQuery += @"
            //        AND a.oid IN (SELECT DISTINCT oid FROM ncumap WHERE uno = @Uno)
            //        AND b.lactive = '1'
            //        AND a.lcode IN (SELECT DISTINCT lcode FROM ncumap WHERE uno = @Uno)
            //    ";
            //}

            sqlQuery += " ORDER BY a.doe DESC, b.lname";

            //Third Return Variable= currently acceptable and generated a new error of (SqlException: Conversion failed when converting the nvarchar value 'adm' to data type int)
           // var result = await _context.RegistrationViewModel.FromSqlRaw(sqlQuery, new SqlParameter("@Uno", uno))
                            //.ToListAsync();
                
            //first return variable=
            var result = await _context.RegistrationViewModel.FromSqlRaw(sqlQuery, uno).ToListAsync();
            //Second return variables=
            //var result = await _context.RegistrationViewModel.FromSqlRaw(sqlQuery, new SqlParameter("@Uno", uno)).ToListAsync();
            return result;
        }
    }
}
