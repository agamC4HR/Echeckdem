using DocumentFormat.OpenXml.Office2013.Excel;
using Echeckdem.Models;
using Echeckdem.CustomFolder;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.InkML;
using System.Linq;
using Amazon.Runtime;


namespace Echeckdem.Services
{
    public class SiteManagementService : ISiteManagementService
    {
        private readonly DbEcheckContext _dbEcheckContext;
        private readonly ILogger<SiteManagementService> _logger;

        public SiteManagementService(DbEcheckContext dbEcheckContext, ILogger<SiteManagementService> logger)
        {
            _dbEcheckContext = dbEcheckContext;
            _logger = logger;
        }

        public async Task<List<OrganisationsListViewModel>> GetActiveOrganizationsAsync()
        {
            return await _dbEcheckContext.Ncmorgs
                .Where(o => o.Oactive == 1)
                .Select(o => new OrganisationsListViewModel
                {
                    oid = o.Oid,
                    Oname = o.Oname
                })
                .ToListAsync();
        }

        public async Task<List<LocationViewModel>> GetLocationsByOidAsync(string oid)
        {
            return await _dbEcheckContext.Ncmlocs
                .Where(l => l.Oid == oid)
                .Select(l => new LocationViewModel
                {
                    Oid = l.Oid,
                    Lcode = l.Lcode,
                    Lname = l.Lname,
                    Lstate = l.Lstate,
                    Ltype = l.Ltype,
                    Iscloc = l.Iscloc,
                    Iscentral = l.Iscentral,
                    Lregion = l.Lregion,
                    Laddress = l.Laddress,
                    Lcontact = l.Lcontact,
                    Lconno = l.Lconno,
                    Lconemail = l.Lconemail,
                    Cemail = l.Cemail,
                    Iemail = l.Iemail
                })
                .ToListAsync();
        }

        public async Task<LocationViewModel?> GetLocationDetailsAsync(string oid, string lcode)
        {
            return await _dbEcheckContext.Ncmlocs
                .Where(l => l.Oid == oid && l.Lcode == lcode)
                .Select(l => new LocationViewModel
                {
                    Oid = l.Oid,
                    Lcode = l.Lcode,
                    Lstate = l.Lstate,
                    Ltype = l.Ltype,
                    Iscloc = l.Iscloc
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<ReturnTemplateViewModel>> GetApplicableReturnsAsync(ReturnPeriodSelectionViewModel input)
        {
            var query = _dbEcheckContext.Nctemprets
                .Where(r => r.Rstate == input.Lstate && r.Ractive == 1 && r.Rm >= input.Month);

            var ltypeTrimmed = input.Ltype?.Trim();

            if (ltypeTrimmed == "S" && input.Iscloc == 0)
            {
                query = query.Where(r => !new[] { "F", "I", "P" }.Contains(r.Rtype));                   
            }
            else if (ltypeTrimmed == "S" && input.Iscloc == 1)
            {
                query = query.Where(r => !new[] { "F", "I" }.Contains(r.Rtype));
            }
            else if (ltypeTrimmed == "F" && input.Iscloc == 0)
            {
                query = query.Where(r => !new[] { "S", "I", "P" }.Contains(r.Rtype));
            }
            else if (ltypeTrimmed == "F" && input.Iscloc == 1)
            {
                query = query.Where(r => !new[] { "S", "I" }.Contains(r.Rtype));
            }


            var ApplicableReturns = await query
                .Select(r => new ReturnTemplateViewModel
                {
                    Rcode = r.Rcode,
                    Rtitle = r.Rtitle,
                    Rform = r.Rform,
                    Rdesc = r.Rdesc,
                    Rd = r.Rd,
                    Rm = r.Rm,
                    Yroff = r.Yroff,
                    Selected = false
                })
                .ToListAsync();

            return ApplicableReturns;
        }


        public async Task SaveSelectedReturnsAsync(ReturnPeriodSelectionViewModel input)
        {
            var entries = input.ApplicableReturns
           .Where(r => r.Selected && r.Rd.HasValue && r.Rm.HasValue && r.Yroff.HasValue)

           .Select(r =>
           {
               var year = r.Yroff == 1 ? input.Year + 1 : input.Year;

               DateOnly lastDate;
               try
               {
                   lastDate = new DateOnly(year, r.Rm.Value, r.Rd.Value);
               }
               catch
               {
                   // Optionally skip or handle invalid dates
                   return null;
               }

               return new Ncret
               {
                   Oid = input.Oid,
                   Lcode = input.Lcode,
                   Rcode = r.Rcode,
                   Ryear = input.Year,
                   Lastdate = lastDate
               };
           })
           .Where(x => x != null) // remove nulls from invalid dates
           .ToList();

            _dbEcheckContext.Ncrets.AddRange(entries!);
            await _dbEcheckContext.SaveChangesAsync();
        }

        public async Task<List<IGrouping<int, ReturnDetailViewModel>>> GetSubmittedReturnsByOrg(string oid, string lcode)
        {
            var result = await (
                from r in _dbEcheckContext.Ncrets
                join org in _dbEcheckContext.Ncmorgs on r.Oid equals org.Oid
                join loc in _dbEcheckContext.Ncmlocs on new { r.Oid, r.Lcode } equals new { loc.Oid, loc.Lcode }
                join tmpl in _dbEcheckContext.Nctemprets on r.Rcode equals tmpl.Rcode
                where r.Oid == oid && r.Lcode == lcode
                select new ReturnDetailViewModel
                {
                    OrganizationName = org.Oname,
                    LocationName = loc.Lname,
                    Ryear = r.Ryear,
                    LastDate = r.Lastdate,
                    Rtitle = tmpl.Rtitle,
                    Rform = tmpl.Rform,
                    Rdesc = tmpl.Rdesc
                }
            ).ToListAsync();

            var groupedReturns = result
        .GroupBy(r => r.Ryear)
        .ToList();

            return groupedReturns;
        }

        public async Task<List<ContributionTemplateViewModel>> GetApplicableContributionsAsync(ContributionPeriodSelectionViewModel input)
        {
            var query = _dbEcheckContext.Nctempcnts
                .Where(c => c.Cstate == input.Lstate && c.Period >= input.Month && c.Active == 1);

            if (input.SelectedTP != "ALL")
            {
                string trimmedTP = input.SelectedTP.Trim().ToUpper();
                query = query.Where(c => c.Tp.Trim().ToUpper() == trimmedTP);
            }
            else
            {
                var allowedTypes = new[] { "ESI", "PF", "LWF", "PT" };
                query = query.Where(c => allowedTypes.Contains(c.Tp.Trim().ToUpper()));
            }

            var applicableContributions = await query
                .Select(c => new ContributionTemplateViewModel
                {
                    Cid = c.Cid,
                    TP = c.Tp.Trim(),
                    Freq = c.Freq,
                    Ld = c.Ld,
                    Period = c.Period,
                    Moffset = c.Moffset
                })
                .ToListAsync();

            return applicableContributions;
        }

        public async Task SaveSelectedContributionsAsync(ContributionPeriodSelectionViewModel input)
        {
            var entries = input.ApplicableContributions
         .Where(c => c.Selected && c.Ld.HasValue && c.Moffset.HasValue && c.Period.HasValue)
         .Select(c =>
         {
             int dueMonth = c.Moffset == 1 ? c.Period.Value : c.Period.Value + 1;
             int dueYear = input.Year;

             if (dueMonth > 12)
             {
                 dueMonth = 1;
                 dueYear += 1;
             }

             DateOnly lastDate;
             try
             {
                 lastDate = new DateOnly(dueYear, dueMonth, c.Ld.Value);
             }
             catch
             {
                 // Invalid date, e.g. Feb 30
                 return null;
             }

             return new Nccontr
               {
                   Oid = input.Oid,
                   Lcode = input.Lcode,
                   Tp = c.TP,
                   Cyear = input.Year,
                   //Period = c.Period,
                   Freq = c.Freq,
                   Lastdate = lastDate
               };
           })
           .Where(x => x != null) // remove nulls from invalid dates
           .ToList();

            _dbEcheckContext.Nccontrs.AddRange(entries!);
            await _dbEcheckContext.SaveChangesAsync();
        }

        public async Task<List<IGrouping<int, ContributionDetailViewModel>>> GetSubmittedContributionsByOrg(string oid, string lcode)
        {
            var result = await (
                from r in _dbEcheckContext.Nccontrs
                join org in _dbEcheckContext.Ncmorgs on r.Oid equals org.Oid
                join loc in _dbEcheckContext.Ncmlocs on new { r.Oid, r.Lcode } equals new { loc.Oid, loc.Lcode }
                //join tmpl in _dbEcheckContext.Nctempcnts on r.Rcode equals tmpl.Rcode
                where r.Oid == oid && r.Lcode == lcode
                select new ContributionDetailViewModel
                {
                    OrganizationName = org.Oname,
                    LocationName = loc.Lname,
                    Cyear = r.Cyear,
                    LastDate = r.Lastdate,
                    Freq = r.Freq,
                    Tp = r.Tp
                    
                    
                }
            ).ToListAsync();

            var groupedContributions = result
        .GroupBy(r => r.Cyear)
        .ToList();

            return groupedContributions;

        }

    }

}




