using Echeckdem.Models;
using Microsoft.EntityFrameworkCore;
using Echeckdem.CustomFolder.ProjectBocw;


namespace Echeckdem.Services
{
    public class ProjectBocwService
    {
        private readonly DbEcheckContext _dbEcheckContext;
        private static readonly Dictionary<int, string> StatusMap = new()
        {
            [0] = "Docs/Info Awaited",
            [1] = "Under Processing",
            [2] = "A/F",
            [3] = "Received",
            [4] = "Done",
            [5] = "Not-Done",
            [-1] = "Action Awaited",
            [-2] = "Future"
        };
        public ProjectBocwService(DbEcheckContext dbEcheckContext)
        {
            _dbEcheckContext = dbEcheckContext;
        }

        public async Task<Dictionary<string, List<string>>> GetUserOrgSiteMapAsync(int uno)
        {
            var mappings = await (from map in _dbEcheckContext.Ncumaps
                                  join loc in _dbEcheckContext.Ncmlocs on new { map.Lcode, map.Oid } equals new { loc.Lcode, loc.Oid }
                                  join org in _dbEcheckContext.Ncmorgs on loc.Oid equals org.Oid
                                  where map.Uno == uno && loc.Ltype == "BO"
                                  select new
                                  {
                                      OrgName = org.Oname,
                                      SiteName = loc.Lname
                                  }).ToListAsync();

            return mappings
                .GroupBy(m => m.OrgName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.SiteName).Distinct().ToList()
                );
        }

        public async Task<ProjectDetailsDto> GetProjectDetailsAsync(int uno, string clientName, string siteName)
        {
            var map = await _dbEcheckContext.Ncumaps
                .Where(m => m.Uno == uno)
                .Join(_dbEcheckContext.Ncmorgs,
                    m => m.Oid,
                    o => o.Oid,
                    (m, o) => new { m.Lcode, m.Oid, OrgName = o.Oname })
                .Join(_dbEcheckContext.Ncmlocs,
                    mo => new { mo.Lcode, mo.Oid },
                    l => new { l.Lcode, l.Oid },
                    (mo, l) => new { mo.Lcode, mo.Oid, mo.OrgName, SiteName = l.Lname, l.Lstate, l.Lcity })
                .Where(x => x.OrgName == clientName && x.SiteName == siteName)
                .FirstOrDefaultAsync();

            if (map == null)
                return null;

            var locb = await _dbEcheckContext.Ncmlocbos
                .Where(b => b.Lcode == map.Lcode)
                .FirstOrDefaultAsync();

            return new ProjectDetailsDto
            {
                SiteName = map.SiteName,
                State = map.Lstate,
                City = map.Lcity,
                ClientName = locb?.ClientName,
                GeneralContractor = locb?.GeneralContractor,
                ProjectStartDate = locb?.ProjectStartDateEst?.ToString("dd-MMM-yyyy"),
                ProjectEndDate = locb?.ProjectEndDateEst?.ToString("dd-MMM-yyyy"),
                ProjectArea = locb?.ProjectArea,
                ProjectCost = locb?.ProjectCostEst?.ToString("N2"),
                ProjectLead = locb?.ProjectLead
            };
        }
        public async Task<List<ComplianceActivityDto>> GetComplianceActivitiesAsync(string clientName, string siteName)
        {
            var map = await (from m in _dbEcheckContext.Ncmorgs
                             join l in _dbEcheckContext.Ncmlocs on m.Oid equals l.Oid
                             where m.Oname == clientName && l.Lname == siteName && l.Ltype == "BO"
                             select new { l.Lcode, m.Oid }).FirstOrDefaultAsync();

            if (map == null)
                return new List<ComplianceActivityDto>();


            var bocwEntries = _dbEcheckContext.Ncbocws

     //.Where(b => b.Lcode == "14860f99")
.Where(b => b.Lcode == map.Lcode)
     .Select(b => b.TransactionId)

     .ToList();
            Console.Write(bocwEntries);

            Dictionary<int, int> acidMap = new();
            try
            {
                var actionGroups = _dbEcheckContext.Ncactions

 .Where(a => a.Aclink.HasValue && bocwEntries.Contains(a.Aclink.Value))

 .ToList();

                acidMap = actionGroups
                            .GroupBy(a => a.Aclink.Value)
                            .ToDictionary(g => g.Key, g => g.First().Acid);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while building acidMap: " + ex.Message);
                throw;
            }



            //Console.Write(actionGroups);

            //var bocwEntries = await _dbEcheckContext.Ncbocws
            //    .Where(b => b.Lcode == map.Lcode)
            //    .OrderBy(b => b.TransactionId)
            //    .ToListAsync();

            // var transactionIds = bocwEntries.Select(b => b.TransactionId).ToList();



            //        Dictionary<int, int> acidMap = new();
            //        try
            //        {
            //            //var materializedTransactionIds = transactionIds.ToList();
            //            var actionGroups = await _dbEcheckContext.Ncactions
            //.Where(a => a.Aclink.HasValue && transactionIds.Any(tid => tid == a.Aclink.Value))
            ////.Where(a => a.Aclink.HasValue && materializedTransactionIds.Contains(a.Aclink.Value))
            //.ToListAsync();

            //            acidMap = actionGroups
            //                .GroupBy(a => a.Aclink.Value)
            //                .ToDictionary(g => g.Key, g => g.First().Acid);

            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine("Error while building acidMap: " + ex.Message);
            //            throw;
            //        }




            var fileMap = new Dictionary<int, string>();

            var acidList = acidMap.Values.OfType<int>().ToList();


            if (acidList.Any())
            {
                fileMap = await _dbEcheckContext.Ncfiles
                    .Where(f => acidList.Contains(f.Flink ?? -1))
                    .ToDictionaryAsync(f => f.Flink ?? -1, f => f.Fname);
            }

            //var activities = bocwEntries.Select((entry, index) =>
            //{
            //    string fileName = null;
            //    bool FileExists = false;
            //    if (acidMap.TryGetValue( out int acid))// && acid.HasValue)
            //    {
            //        // Now acid.Value is guaranteed to be a non-nullable integer
            //        FileExists =  fileMap.TryGetValue(acid, out fileName);
            //    }

            //    //string fileUrl = string.IsNullOrEmpty(fileName)
            //    //    ? null
            //    //    : $"/Files/{map.Oid}/Bocw/{fileName}";

            //    string fileUrl = null;
            //    if (!string.IsNullOrEmpty(fileName))
            //    {
            //        fileUrl = "/Files/" + map.Oid + "/Bocw/" + fileName;
            //    }


            //    return new ComplianceActivityDto
            //    {
            //        SNo = index + 1,
            //        ServiceType = "BOCW",
            //        Service = entry.Task,
            //        DueDate = entry.DueDate.ToString("dd-MMM-yyyy"),
            //        Status = StatusMap.TryGetValue(entry.Status, out var statusText) ? statusText : "Unknown",


            //        CompletionDate = entry.CompletionDate.HasValue? entry.CompletionDate.Value.ToString("dd-MMM-yyyy") : null,
            //        FileName = fileName,
            //        FileUrl = fileUrl,
            //        TransactionId = entry.TransactionId,
            //        FileExists = FileExists
            //    };
            //}).ToList();

            //return activities;
            List<ComplianceActivityDto> comp = new List<ComplianceActivityDto>();
            return comp;
        }

    }
}
