﻿using DocumentFormat.OpenXml.Office2013.Excel;
using Echeckdem.Models;
using Echeckdem.CustomFolder;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.InkML;
using System.Linq;


namespace Echeckdem.Services
{
    public class SiteManagementService : ISiteManagementService
    {
        private readonly DbEcheckContext _dbEcheckContext;

        public SiteManagementService(DbEcheckContext dbEcheckContext)
        {
            _dbEcheckContext = dbEcheckContext;
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

       
    }
}
