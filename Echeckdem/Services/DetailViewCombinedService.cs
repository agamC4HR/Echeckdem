using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Echeckdem.Services
{
    public class DetailViewCombinedService
    {
        private readonly DbEcheckContext _dbEcheckContext;

        public DetailViewCombinedService(DbEcheckContext dbEcheckContext)
        {
            _dbEcheckContext = dbEcheckContext;
        }

        public async Task<List<DetailViewCombinedViewModel>> GetDataAsync(
            int ulev, int uno,
            string organizationName = null, string locationName = null, string stateName = null, string cityName = null,
            DateOnly? startDueDate = null, DateOnly? endDueDate = null, DateOnly? startPeriod = null, DateOnly? endPeriod = null)
        {
            var sqlQuery = @"
                SELECT * FROM (
                    -- Registration Data
                    SELECT a.oid, a.doe AS Depdate, a.Status, a.dolr AS Lastdate, a.tp,
                           b.lname, b.lstate, b.lcity, b.lregion,
                           c.oname, e.statedesc AS State,
                           'Registration' AS RecordType,
                           NULL AS Period, NULL AS Cyear
                    FROM ncreg a
                    JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
                    JOIN ncmorg c ON c.oid = b.oid
                    JOIN MASTSTATES e ON b.lstate = e.stateid
                    WHERE c.oactive = 1 AND a.status <> 99
                    
                    UNION ALL
                    
                    -- Contribution Data 
                    SELECT a.oid, a.tp, a.Status, a.depdate, a.lastdate,
                           b.lname, b.lstate, b.lcity, b.lregion, 
                           c.oname, e.statedesc AS State,
                           'Contribution' AS RecordType,
                           a.Period, a.Cyear
                    FROM nccontr a 
                    JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
                    JOIN ncmorg c ON c.oid = b.oid
                    JOIN MASTSTATES e ON b.lstate = e.stateid
                    WHERE c.oactive = 1 AND a.status <> 99
                    
                    UNION ALL
                    
                    -- Return Data
                    SELECT a.oid, a.depdate, a.status, a.lastdate,
                           b.lname, b.lstate, b.lcity, b.lregion, 
                           d.oname, e.statedesc AS State,
                           'Return' AS RecordType,
                           NULL AS Period, NULL AS Cyear
                    FROM ncret a
                    JOIN ncmloc b ON a.lcode = b.lcode AND a.oid = b.oid
                    JOIN ncmorg d ON b.oid = d.oid
                    JOIN MASTSTATES e ON b.lstate = e.stateid
                    WHERE d.oactive = 1 AND a.status <> 99
                ) AS CombinedData
                WHERE (oname = @organizationName OR @organizationName IS NULL)
                  AND (lname = @locationName OR @locationName IS NULL)
                  AND (State = @stateName OR @stateName IS NULL)
                  AND (lcity = @cityName OR @cityName IS NULL)
                  AND (lastdate >= @startDueDate OR @startDueDate IS NULL)
                  AND (lastdate <= @endDueDate OR @endDueDate IS NULL)
                  AND (depdate >= @startPeriod OR @startPeriod IS NULL)
                  AND (depdate <= @endPeriod OR @endPeriod IS NULL)
                ORDER BY lastdate DESC, lname";

            var result = await _dbEcheckContext.Database.SqlQueryRaw<DetailViewCombinedViewModel>(sqlQuery,
                new SqlParameter("@organizationName", (object)organizationName ?? DBNull.Value),
                new SqlParameter("@locationName", (object)locationName ?? DBNull.Value),
                new SqlParameter("@stateName", (object)stateName ?? DBNull.Value),
                new SqlParameter("@cityName", (object)cityName ?? DBNull.Value),
                new SqlParameter("@startDueDate", (object)startDueDate ?? DBNull.Value),
                new SqlParameter("@endDueDate", (object)endDueDate ?? DBNull.Value),
                new SqlParameter("@startPeriod", (object)startPeriod ?? DBNull.Value),
                new SqlParameter("@endPeriod", (object)endPeriod ?? DBNull.Value))
                .ToListAsync();

            return result;
        }
    }
}
