using Echeckdem.Models;
using Microsoft.EntityFrameworkCore;
using Echeckdem.CustomFolder.ProjectBocw;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Echeckdem.CustomFolder;



namespace Echeckdem.Services
{
    public class ProjectBocwService
    {
        private readonly DbEcheckContext _dbEcheckContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
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
        public ProjectBocwService(DbEcheckContext dbEcheckContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbEcheckContext = dbEcheckContext;
            _httpContextAccessor = httpContextAccessor;
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

            // Fetch BOCW data
            var bocwRecords = await _dbEcheckContext.Ncbocws
                .Where(b => b.Lcode == map.Lcode)
                .ToListAsync();

            var scopeIds = bocwRecords.Select(b => b.ScopeId).Distinct().ToList();


            var bocwDtos = bocwRecords.Select(b => new BocwServiceDto
            {
                Service = b.Task,

                Category = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == b.ScopeId).Select(a => a.Category).FirstOrDefault(),
                DueDate = b.DueDate,
                Status = StatusMap.ContainsKey(b.Status) ? StatusMap[b.Status] : "Unknown",
                CompletionDate = b.CompletionDate,
                File = b.FileName,
                transactionID = b.TransactionId,
                lcode = b.Lcode
            }).ToList();

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
                ProjectLead = locb?.ProjectLead,
                BocwServices = bocwDtos
            };
        }

        



    }
}
